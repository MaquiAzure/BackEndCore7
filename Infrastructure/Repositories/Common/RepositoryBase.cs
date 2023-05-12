namespace Infrastructure.Repositories.Common
{
    using Application.Contracts.Repositories.Base;
    using Domain.Entities.Common;
    using Infrastructure.Persistence.Contexts;
    using Microsoft.EntityFrameworkCore;
    using System.Linq.Expressions;

    public class RepositoryBase<T> : IRepositoryBase<T> where T : BaseDomainModel
    {
        protected readonly ApplicationDbContext _context;

        public RepositoryBase(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }
        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicado)
        {
            return await _context.Set<T>().Where(predicado).ToListAsync();
        }
        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicado = null,
                                                    Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                                    string? includeString = null,
                                                    bool disableTracking = true)
        {
            IQueryable<T> query = _context.Set<T>();
            if (disableTracking) query = query.AsNoTracking();
            if (string.IsNullOrWhiteSpace(includeString)) query = query.Include(includeString!);
            if (predicado != null) query = query.Where(predicado);
            if (orderBy != null) return await orderBy(query).ToListAsync();
            return await query.ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicado = null,
                                                       Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                                       List<Expression<Func<T, object>>>? includes = null,
                                                       bool disableTracking = true)
        {
            IQueryable<T> query = _context.Set<T>();
            if (disableTracking) query = query.AsNoTracking();
            if (includes != null) query = includes.Aggregate(query, (current, include) => current.Include(include));
            if (predicado != null) query = query.Where(predicado);
            if (orderBy != null) return await orderBy(query).ToListAsync();
            return await query.ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicado = null,
                                                List<Expression<Func<T, object>>>? includes = null,
                                                bool disableTracking = true)
        {
            IQueryable<T> query = _context.Set<T>();
            if (disableTracking) query = query.AsNoTracking();
            if (includes != null) query = includes.Aggregate(query, (current, include) => current.Include(include));
            if (predicado != null) query = query.Where(predicado);
            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return (await _context.Set<T>().FindAsync(id))!;
        }
        public async Task<T> GetByIdAsync(Guid id, List<Expression<Func<T, object>>>? includes = null)
        {
            var query = _context.Set<T>().AsQueryable();
            if (includes != null) query = includes.Aggregate(query, (current, include) => current.Include(include));
            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        public async Task<T> AddAsync(T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<T> UpdateAsync(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public void AddEntity(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void UpdateEntity(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void DeleteEntity(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
        public void AddRange(List<T> entities)
        {
            _context.Set<T>().AddRange(entities);
        }

        public void DeleteRange(List<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

    }
}
