
using Backoffice.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Backoffice.Api.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
   private readonly BackofficeDbContext _context;
   private readonly DbSet<T> _dbSet;

   public Repository(BackofficeDbContext context)
   {
      _context = context;
      _dbSet = context.Set<T>();
   }

   public async Task<List<T>> GetAllAsync() => await _dbSet.ToListAsync();
   public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);
   public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
   public void Delete(T entity) => _dbSet.Remove(entity);
   public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
   public async Task<T?> FindAsync(Func<T, bool> predicate) =>
    await Task.Run(() => _dbSet.FirstOrDefault(predicate));
}
