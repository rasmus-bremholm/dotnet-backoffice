using System.Linq.Expressions;

namespace Backoffice.Api.Repositories;

public interface IRepository<T>
{
   Task<List<T>> GetAllAsync();
   Task<T?> GetByIdAsync(int id);
   Task AddAsync(T entity);
   void Delete(T entity);
   Task SaveChangesAsync();
   Task<T?> FindAsync(Expression<Func<T, bool>> predicate);
}
