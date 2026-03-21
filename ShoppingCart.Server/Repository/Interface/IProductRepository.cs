using System.Collections.Generic;
using System.Threading.Tasks;
using AngularApplication.Models;

namespace ShoppingCartAPI.Repository.Interface
{
    public interface IProductRepository
    {
        Task<Product> CreateAsync(Product product);
        Task<Product?> GetByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> UpdateAsync(Product product);
        Task<bool> DeleteAsync(int id);
        Task<ProductImage?> AddImageAsync(int productId, ProductImage image);
        Task<bool> RemoveImageAsync(int productId, int imageId);
    }
}