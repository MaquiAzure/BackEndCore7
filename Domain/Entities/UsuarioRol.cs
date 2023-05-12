using Domain.Entities.Common;
namespace Domain.Entities
{
    public class UsuarioRol : BaseDomainModel
    {
        public Guid UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public Guid RolId { get; set; }
        public Rol? Rol { get; set; }
    }
}
