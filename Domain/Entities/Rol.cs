
namespace Domain.Entities
{
    using Domain.Entities.Common;

    public class Rol : BaseDomainModel
    {
        public string Name { get; set; }
        public ICollection<UsuarioRol> UsuarioRoles { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
