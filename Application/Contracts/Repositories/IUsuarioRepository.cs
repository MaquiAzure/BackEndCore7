namespace Application.Contracts.Repositories
{
    using Application.Contracts.Repositories.Base;
    using Domain.Entities;
    public interface IUsuarioRepository : IRepositoryBase<Usuario>
    {
        Task<Usuario> FindByEmailAsync(string email); 
        Task<Usuario> FindByIdUsuario(Guid Id);
        Task<bool> CheckPasswordAsync(Usuario usuario, string password);
        Task<List<Rol>> GetRolesByUserIDAsync(Guid usuarioId);
        Task AddToRoleAsync(Usuario usuario, string roleName);
    }
}
