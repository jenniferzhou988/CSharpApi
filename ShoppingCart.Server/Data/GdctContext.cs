using AngularApplication.Models;
using Microsoft.EntityFrameworkCore;
using ShoppingCartAPI.Models;

namespace ShoppingCartAPI.Data;

public partial class GdctContext(DbContextOptions<GdctContext> options) : DbContext(options)
{
    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<AppConfig> AppConfigs { get; set; }
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public virtual  DbSet<Product> Products { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("UserInfo");

            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.FullName).HasMaxLength(100);
            
            entity.HasOne(d => d.UserRole).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserRoleId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_UserInfo_UserRole");

        });

        modelBuilder.Entity<Product>(entity => {
            entity.ToTable("Products");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).ValueGeneratedOnAdd();
            entity.Property(p => p.ProductName)
                   .IsRequired()
                   .HasMaxLength(200);
            entity.Property(p => p.Price)
                   .HasColumnType("decimal(18,2)");
            entity.Property(p => p.Description)
                   .HasMaxLength(1000);
            entity.Property(p => p.Created)
                   .IsRequired();
            entity.Property(p => p.CreatedBy)
                   .HasMaxLength(256);
            entity.Property(p => p.Modified);
            entity.Property(p => p.ModifiedBy)
                   .HasMaxLength(256);

        });

        modelBuilder.Entity<RefreshToken>()
                 .HasIndex(rt => rt.TokenHash)
                 .IsUnique();

        modelBuilder.Entity<RefreshToken>()
            .HasOne(rt => rt.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId);



        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.ToTable("UserRole");

            entity.Property(e => e.RoleName).HasMaxLength(50);
        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}