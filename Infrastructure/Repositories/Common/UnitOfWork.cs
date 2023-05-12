namespace Infrastructure.Repositories.Common
{
    using Application.Contracts.Repositories.Base;
    using Application.Contracts.Repositories;
    using Domain.Entities.Common;
    using System.Collections;
    using Infrastructure.Persistence.Contexts;

    public class UnitOfWork : IUnitOfWork
    {
        private Hashtable _repositories;
        private readonly ApplicationDbContext _context;

        private IUsuarioRepository _usuarioRepository;
        private IRolRepository _rolRepository;
        public IUsuarioRepository UsuarioRepository => _usuarioRepository ??= new UsuarioRepository(_context, RolRepository);
        public IRolRepository RolRepository => _rolRepository ??= new RolRepository(_context);

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Complete()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception("Error en Transacción ", e);
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IRepositoryBase<TEntity> Repository<TEntity>() where TEntity : BaseDomainModel
        {
            if (_repositories == null)
            {
                _repositories = new Hashtable();
            }
            var type = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(RepositoryBase<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);
                _repositories.Add(type, repositoryInstance);
            }
            return (IRepositoryBase<TEntity>)_repositories[type];
        }
    }
}
