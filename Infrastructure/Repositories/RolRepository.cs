namespace Infrastructure.Repositories
{
    using Application.Contracts.Repositories.Base;
    using Application.Contracts.Repositories;
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Infrastructure.Repositories.Common;
    using Infrastructure.Persistence.Contexts;
    public class RolRepository : RepositoryBase<Rol>, IRolRepository
    {
        public RolRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task AddToRoleAsync(Usuario usuario, string roleName)
        {
            if (usuario == null)
            {
                throw new ArgumentNullException(nameof(usuario));
            }

            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("Valor no puede ser nulo o vacio.", nameof(roleName));
            }

            var rol = await _context.Roles.Where(r => r.Name == roleName).FirstOrDefaultAsync();
            if (rol == null)
            {
                throw new InvalidOperationException($"Role '{roleName}' not found.");
            }
            var userRole = new UsuarioRol
            {
                //UsuarioId=usuario.Id,
                //RolId=rol.Id,
                Usuario = usuario,
                Rol = rol
            };

            _context.UsuarioRoles.Add(userRole);
        }
    }
}
