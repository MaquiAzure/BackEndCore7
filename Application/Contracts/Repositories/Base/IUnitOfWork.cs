namespace Application.Contracts.Repositories.Base
{
    using Domain.Entities.Common;
    public interface IUnitOfWork : IDisposable
    {
        IRepositoryBase<TEntity> Repository<TEntity>() where TEntity : BaseDomainModel;
        Task<int> Complete();
        public IUsuarioRepository UsuarioRepository { get; }
        public IRolRepository RolRepository { get; }

    }
}
