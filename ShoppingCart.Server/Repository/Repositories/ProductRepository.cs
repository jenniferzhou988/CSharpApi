using AngularApplication.Models;
using Microsoft.EntityFrameworkCore;
using ShoppingCartAPI.Data;
using ShoppingCartAPI.Repository.Interface;

namespace ShoppingCartAPI.Repository.Repositories
{
    public class ProductRepository : GDCTRepository<Product>,IProductRepository
    {
       // private readonly ApplicationDbContext _db;

        public ProductRepository(IDbContextFactory<GdctContext> dbcontextfactory, IAppLogger<Product> logger) : base(dbcontextfactory, logger)
        {
         //   _db = db;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            using (var ctx = _dbcontextfactory.CreateDbContext())
            {
                ctx.Products.Add(product);
                await ctx.SaveChangesAsync();
                return product;
            }
            
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            using (var ctx = _dbcontextfactory.CreateDbContext())
            {
                return await ctx.Products.FindAsync(id);
            }
                
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            using (var ctx = _dbcontextfactory.CreateDbContext())
            {
                return await ctx.Products.AsNoTracking().ToListAsync();
            }
             
        }

        public async Task<Product?> UpdateAsync(Product product)
        {
            using (var ctx = _dbcontextfactory.CreateDbContext())
            {
                var existing = await ctx.Products.FindAsync(product.Id);
                if (existing == null)
                    return null;

                existing.ProductName = product.ProductName;
                existing.Price = product.Price;
                existing.Description = product.Description;
                // Modified and ModifiedBy are handled by ApplicationDbContext.ApplyAuditInformation/save pipeline
                ctx.Products.Update(existing);
                await ctx.SaveChangesAsync();
                return existing;
            }
                
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var ctx = _dbcontextfactory.CreateDbContext())
            {
                var existing = await ctx.Products.FindAsync(id);
                if (existing == null)
                    return false;

                ctx.Products.Remove(existing);
                await ctx.SaveChangesAsync();
                return true;

            }
                
        }
    }
}