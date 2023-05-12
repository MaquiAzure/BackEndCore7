namespace Infrastructure.Repositories
{
    using Application.Contracts.Repositories;
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Infrastructure.Repositories.Common;
    using Infrastructure.Persistence.Contexts;
    public class UsuarioRepository : RepositoryBase<Usuario>, IUsuarioRepository
    {
        private readonly IRolRepository _rolRepository;

        public UsuarioRepository(ApplicationDbContext context, IRolRepository rolRepository) : base(context)
        {
            _rolRepository = rolRepository;
        }

        public async Task<bool> CheckPasswordAsync(Usuario usuario, string password)
        {
            if (usuario.PasswordHash.Equals(password))
            {
                return true;
            }
            return false;
        }

        public async Task<Usuario> FindByEmailAsync(string email)
        {
            var usuario = await _context.Usuarios.Where(usuario => usuario.Email == email && usuario.isActive == true).FirstOrDefaultAsync();
            return usuario;
        }
        public async Task<Usuario> FindByIdUsuario(string Id)
        {
            var usuario = await _context.Usuarios.Where(usuario => usuario.Id.ToString().ToUpper().Contains(Id) && usuario.isActive == true).FirstOrDefaultAsync();
            return usuario;
        }
        public async Task<List<Rol>> GetRolesByUserIDAsync(Guid usuarioId)
        {
            var roles = await _context.Roles
                .Where(r => r.Usuarios.Any(u => u.Id == usuarioId))
                .ToListAsync();

            return roles;
        }
        public async Task AddToRoleAsync(Usuario usuario, string roleName)
        {
            if (usuario == null)
            {
                throw new ArgumentNullException(nameof(usuario));
            }
            var rolesByUsuario = await GetRolesByUserIDAsync(usuario.Id);

            if (rolesByUsuario.Any(r => r.Name == roleName))
            {
                return;
            }

            await _rolRepository.AddToRoleAsync(usuario, roleName);
        }
    }
}
