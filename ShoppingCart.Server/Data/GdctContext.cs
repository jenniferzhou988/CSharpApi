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
    public virtual DbSet<ProductImage> ProductImages { get; set; } = null!;

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

            entity.HasMany(p => p.Images)
                   .WithOne(i => i.Product)
                   .HasForeignKey(i => i.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ProductImage>(entity => {
            entity.ToTable("ProductImages");
            entity.HasKey(i => i.Id);
            entity.Property(i => i.Id).ValueGeneratedOnAdd();
            entity.Property(i => i.ImageUrl)
                   .IsRequired()
                   .HasMaxLength(500);
            entity.Property(i => i.AltText)
                   .HasMaxLength(200);
            entity.Property(i => i.Created)
                   .IsRequired();
            entity.Property(i => i.CreatedBy)
                   .HasMaxLength(256);
            entity.Property(i => i.Modified);
            entity.Property(i => i.ModifiedBy)
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