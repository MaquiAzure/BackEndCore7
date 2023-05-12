namespace Infrastructure.Persistence.Configuration
{
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuario");

            builder.Property(p => p.Id).IsRequired();

            builder.HasKey(p => p.Id);

            /*
            Relación de Muchos a Muchos
            */
            builder
                .HasMany(usuario => usuario.Roles)
                .WithMany(rol => rol.Usuarios)
                .UsingEntity<UsuarioRol>(
                    usuarioRol =>
                            usuarioRol.HasOne(ur => ur.Rol)
                                .WithMany(r => r.UsuarioRoles)
                                .HasForeignKey(ur => ur.RolId),
                    usuarioRol =>
                                usuarioRol.HasOne(ur => ur.Usuario)
                                    .WithMany(u => u.UsuarioRoles)
                                    .HasForeignKey(ur => ur.UsuarioId),
                    usuarioRol =>
                    {
                        usuarioRol.HasKey(ur => ur.Id);
                    }
                 );
        }
    }
}
