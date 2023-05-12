namespace Application.Contracts.Repositories.Base
{
    using Domain.Entities.Common;
    using System.Linq.Expressions;
    public interface IRepositoryBase<T> where T : BaseDomainModel
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicado);

        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicado = null,
                                        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                        string? includeString = null,
                                        bool disableTracking = true);

        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicado = null,
                                        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                        List<Expression<Func<T, object>>>? includes = null,
                                        bool disableTracking = true);

        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicado = null,
                                        List<Expression<Func<T, object>>>? includes = null,
                                        bool disableTracking = true);
        Task<T> GetByIdAsync(Guid id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        void AddEntity(T entity);
        void UpdateEntity(T entity);
        void DeleteEntity(T entity);
        void AddRange(List<T> entities);
        void DeleteRange(List<T> entities);
    }
}
