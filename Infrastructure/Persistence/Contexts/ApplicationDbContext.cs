namespace Infrastructure.Persistence.Contexts
{
    using Domain.Entities;
    using Infrastructure.Persistence.Interceptor;
    using Microsoft.EntityFrameworkCore;
    using System.Reflection;

    public class ApplicationDbContext : DbContext
    {
        public readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
                AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor
            ) : base(options)
        {
            _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
            optionsBuilder.EnableSensitiveDataLogging();

            optionsBuilder.EnableSensitiveDataLogging();
        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<UsuarioRol> UsuarioRoles { get; set; }
        public DbSet<Auditoria> Auditorias { get; set; }

    }
}
