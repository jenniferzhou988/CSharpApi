using ShoppingCartAPI.Models;

namespace ShoppingCartAPI.Repository.Interface
{
    public interface IShoppingCartRepository : IGDCTRepository<ShoppingCart>
    {
        Task<ShoppingCart> CreateAsync(int customerId);
        Task<ShoppingCart?> GetByIdAsync(int id);
        Task<IEnumerable<ShoppingCart>> GetAllAsync();
        Task<bool> DeleteAsync(int id);
        Task<ShoppingCartDetail?> AddItemAsync(int shoppingCartId, int productId, int quantity);
        Task<ShoppingCartDetail?> UpdateItemAsync(int shoppingCartId, int detailId, int quantity);
        Task<bool> RemoveItemAsync(int shoppingCartId, int detailId);
    }
}