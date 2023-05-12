namespace Application.Contracts.Repositories
{
    using Application.Contracts.Repositories.Base;
    using Domain.Entities;
    public interface IRolRepository : IRepositoryBase<Rol>
    {
        Task AddToRoleAsync(Usuario usuario, string roleName);
    }
}
