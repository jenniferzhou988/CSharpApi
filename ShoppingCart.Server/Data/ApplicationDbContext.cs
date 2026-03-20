using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AngularApplication.Models;

namespace AngularApplication.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var product = modelBuilder.Entity<Product>();

            product.HasKey(p => p.Id);
            product.Property(p => p.Id).ValueGeneratedOnAdd();

            product.Property(p => p.ProductName)
                   .IsRequired()
                   .HasMaxLength(200);

            product.Property(p => p.Price)
                   .HasColumnType("decimal(18,2)");

            product.Property(p => p.Description)
                   .HasMaxLength(1000);

            product.Property(p => p.Created)
                   .IsRequired();

            product.Property(p => p.CreatedBy)
                   .HasMaxLength(256);

            product.Property(p => p.Modified);

            product.Property(p => p.ModifiedBy)
                   .HasMaxLength(256);
        }

        public override int SaveChanges()
        {
            ApplyAuditInformation();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditInformation();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyAuditInformation()
        {
            var utcNow = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<Product>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property(p => p.Created).CurrentValue = utcNow;
                    // set CreatedBy here if you have current user info
                    // entry.Property(p => p.CreatedBy).CurrentValue = currentUser;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property(p => p.Modified).CurrentValue = utcNow;
                    // set ModifiedBy here if you have current user info
                    // entry.Property(p => p.ModifiedBy).CurrentValue = currentUser;
                }
            }
        }
    }
}