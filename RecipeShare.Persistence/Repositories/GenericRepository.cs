using Microsoft.EntityFrameworkCore;
using RecipeShare.Application.Contracts.Persistence;
using RecipeShare.Domain.Common;
using RecipeShare.Persistence.DatabaseContext;

namespace RecipeShare.Persistence.Repositories
{
    /// <summary>
    /// Generic repositoty that can be used for easy extension if and when the application grows and becomes complex. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly RecipeDatabaseContext _context;

        public GenericRepository(RecipeDatabaseContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(T entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
