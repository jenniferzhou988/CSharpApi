using ShoppingCartAPI.Models;

namespace ShoppingCartAPI.Repository.Interface
{
    public interface ICustomerRepository : IGDCTRepository<Customer>
    {
        Task<Customer> CreateAsync(Customer customer);
        Task<Customer?> GetByIdAsync(int id);
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer?> UpdateAsync(Customer customer);
        Task<bool> DeleteAsync(int id);
        Task<CustomerAddressLink?> AddAddressAsync(int customerId, Address address, int addressTypeId);
        Task<CustomerAddressLink?> UpdateAddressAsync(int customerId, int addressLinkId, Address address, int addressTypeId);
        Task<bool> RemoveAddressAsync(int customerId, int addressLinkId);
    }
}