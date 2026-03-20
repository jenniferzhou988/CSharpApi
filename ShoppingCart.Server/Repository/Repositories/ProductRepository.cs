using Microsoft.EntityFrameworkCore;
using AngularApplication.Data;
using AngularApplication.Models;
using ShoppingCartAPI.Repository.Interface;

namespace ShoppingCartAPI.Repository.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            _db.Products.Add(product);
            await _db.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _db.Products.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _db.Products.AsNoTracking().ToListAsync();
        }

        public async Task<Product?> UpdateAsync(Product product)
        {
            var existing = await _db.Products.FindAsync(product.Id);
            if (existing == null)
                return null;

            existing.ProductName = product.ProductName;
            existing.Price = product.Price;
            existing.Description = product.Description;
            // Modified and ModifiedBy are handled by ApplicationDbContext.ApplyAuditInformation/save pipeline
            _db.Products.Update(existing);
            await _db.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _db.Products.FindAsync(id);
            if (existing == null)
                return false;

            _db.Products.Remove(existing);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}