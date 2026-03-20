using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ShoppingCartAPI.Data;
using ShoppingCartAPI.Models;
using ShoppingCartAPI.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// 1) Configuration binding
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

// 2) EF Core
builder.Services.AddDbContext<GdctContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("GDCTConnection")));

// 3) AuthN: JWT Bearer
var jwtSection = builder.Configuration.GetSection("Jwt");
var signingKey = jwtSection["SigningKey"] ?? throw new InvalidOperationException("Jwt:SigningKey missing");
var issuer = jwtSection["Issuer"] ?? "DataCollectionApi";
var audience = jwtSection["Audience"] ?? "DataCollectionClients";


builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,

            ValidateAudience = true,
            ValidAudience = audience,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),

            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });
// Authorization
builder.Services.AddAuthorization();

// 4) AuthZ
/*builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.AddPolicy("AdminsOnly", p => p.RequireRole("Admin"));
}); */

// 5) Utilities
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
//builder.Services.AddSingleton<JwtTokenService>();
builder.Services.AddScoped<ITokenService, TokenService>();

// 1) Add CORS
builder.Services.AddCors(opts =>
{
    /*

   opts.AddDefaultPolicy(policy =>
   {
       var origins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
       policy.WithOrigins(origins)
             .AllowAnyHeader()
             .AllowAnyMethod()
             .AllowCredentials();
   });
    */
   opts.AddPolicy("DevCors", policy =>
   {
       policy
           .WithOrigins(
               "https://127.0.0.1:53109", // Angular dev origin (match yours)
               "https://localhost:53109",  // optionally add localhost if you use it
               "http://localhost:4200"
           )
           .AllowAnyHeader()
           .AllowAnyMethod()
           ;
   });
});


// Minimal rate limiting for auth endpoints
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter(policyName: "auth",
        configureOptions: _ => {
            _.AutoReplenishment = true;
            _.PermitLimit = 100;
            _.Window = TimeSpan.FromMinutes(10);
            _.QueueLimit = 0;
        });
    options.AddFixedWindowLimiter(policyName: "login",
        configureOptions: _ => {
            _.AutoReplenishment = true;
            _.PermitLimit = 10;
            _.Window = TimeSpan.FromMinutes(10);
            _.QueueLimit = 0;
        });
});


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// 6) Swagger (with Bearer)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter **Bearer &lt;token&gt;**",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
    };
    c.AddSecurityDefinition("Bearer", securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    });
});


var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();


app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("DevCors");
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();


app.MapFallbackToFile("/index.html");


// Simple health check
app.MapGet("/health", () => Results.Ok(new { ok = true }));



// 7) Minimal API endpoints

// Allow anonymous on auth endpoints
/*
var auth = app.MapGroup("/api/auth").AllowAnonymous();

auth.MapPost("/register", async (RegisterRequest req, GdctContext db, IPasswordHasher<User> hasher) =>
{
    if (string.IsNullOrWhiteSpace(req.Email) || string.IsNullOrWhiteSpace(req.Password))
        return Results.BadRequest("Email and password are required.");

    var exists = await db.Users.AnyAsync(u => u.Email == req.Email);
    if (exists) return Results.Conflict("Email already registered.");

    // need to modify the code and apply detail user information here
    var user = new User
    {
        Email = req.Email.Trim().ToLowerInvariant(),
        FullName = req.FullName,
        FirstName=req.FirstName,
        LastName=req.LastName,
        Status=1,
        OrgId=req.OrgId,
        UserRoleId=1,
       // UserRole = new UserRole { Id =1, RoleName= "Client User" }
    };
    user.PasswordHash = hasher.HashPassword(user, req.Password);

    db.Users.Add(user);
    await db.SaveChangesAsync();

    return Results.Created($"/api/users/{user.Id}", new { user.Id, user.Email, user.FullName });
});

auth.MapPost("/login", async (LoginRequest req, GdctContext db, IPasswordHasher<User> hasher, JwtTokenService tokens) =>
{
    var user = await db.Users.AsNoTracking().Select(static o=>new User() { 
        Id= o.Id,
        Email = o.Email,
        FullName = o.FullName,
        FirstName = o.FirstName,
        LastName=o.LastName,
        PasswordHash=o.PasswordHash,
        Status=1,
        OrgId=o.OrgId,
        UserRoleId=o.UserRoleId,
        UserRole=o.UserRole,
            })
            .SingleOrDefaultAsync(u => 
            u.Email == req.Email.Trim().ToLowerInvariant()
            );

    if (user is null)
        return Results.Unauthorized();

    var verify = hasher.VerifyHashedPassword(user, user.PasswordHash, req.Password);
    if (verify == PasswordVerificationResult.Failed)
        return Results.Unauthorized();

    var (token, exp) = tokens.CreateAccessToken(user);
    return Results.Ok(new AuthResponse(token, exp, user));
});

// Protected endpoints group
var users = app.MapGroup("/api/users").RequireAuthorization();

users.MapGet("/me", async (HttpContext http, GdctContext db) =>
{
    var userId = http.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
              ?? http.User.FindFirst("NameId")?.Value;
    if (string.IsNullOrEmpty(userId)) return Results.Unauthorized();

    var id = int.Parse(userId);
    var user = await db.Users.AsNoTracking().Where(u => u.Id == id)
        .Select(u => new { u.Id, u.Email, u.FirstName, u.LastName, u.FullName, u.UserRole, u.Created })
        .SingleOrDefaultAsync();

    return user is null ? Results.NotFound() : Results.Ok(user);
});
*/
//users.MapGet("/admin/ping", () => Results.Ok("pong (admin)"))
//   .RequireAuthorization("AdminsOnly");


app.Run();
