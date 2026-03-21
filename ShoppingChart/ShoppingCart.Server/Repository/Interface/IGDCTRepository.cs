using System.Linq.Expressions;
using ShoppingCartAPI.Models;

namespace ShoppingCartAPI.Repository.Interface
{
    public interface IGDCTRepository<T> where T : GDCTEntityBase<int>
    {
        Task<IEnumerable<T>> GetPageSizeListAsync(int pageNumber, int pageSize);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);


    }
}
