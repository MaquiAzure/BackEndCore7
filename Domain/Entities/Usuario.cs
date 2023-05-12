namespace Domain.Entities
{
    using Domain.Entities.Common;
    public class Usuario : BaseDomainModel
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public ICollection<UsuarioRol> UsuarioRoles { get; set; }
        public virtual ICollection<Rol>? Roles { get; set; }
    }
}
