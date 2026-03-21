using ShoppingCartAPI.Models;

namespace ShoppingCartAPI.Repository.Interface
{
    public interface IAddressRepository : IGDCTRepository<Address>
    {
        Task<Address> CreateAsync(Address address);
        Task<Address?> GetByIdAsync(int id);
        Task<IEnumerable<Address>> GetAllAsync();
        Task<Address?> UpdateAsync(Address address);
        Task<bool> DeleteAsync(int id);
    }
}