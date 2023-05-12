namespace Infrastructure.Persistence.Configuration
{
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    internal class UsuarioRolesConfiguration : IEntityTypeConfiguration<UsuarioRol>
    {
        public void Configure(EntityTypeBuilder<UsuarioRol> builder)
        {
            builder.ToTable("UsuarioRol");

            builder.Property(p => p.Id).IsRequired();

            builder.HasKey(p => p.Id);

            /*
            Relación de Muchos a Muchos
            */
            builder
                .HasOne(usuarioRol => usuarioRol.Usuario)
                .WithMany(usuario => usuario.UsuarioRoles)
                .HasForeignKey(usuarioRol => usuarioRol.UsuarioId);

            builder
                .HasOne(usuarioRol => usuarioRol.Rol)
                .WithMany(role => role.UsuarioRoles)
                .HasForeignKey(usuarioRol => usuarioRol.RolId);
        }
    }
}
