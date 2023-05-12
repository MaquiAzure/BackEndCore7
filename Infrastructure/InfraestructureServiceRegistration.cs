namespace Infrastructure
{
    using Application.Contracts.Repositories.Base;
    using Application.Contracts.Repositories;
    using Application.Contracts.Service;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Infrastructure.Persistence.Contexts;
    using Infrastructure.Persistence.Interceptor;
    using Infrastructure.Repositories.Common;
    using Infrastructure.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Infrastructure.Services;
    using Infrastructure.Options;

    public static class InfraestructureServiceRegistration
    {
        public static IServiceCollection AddInfraestructureServices(this IServiceCollection services, IConfiguration configuration)
        {

            ConfigureJwtOptions(services);

            services.AddScoped<AuditableEntitySaveChangesInterceptor>();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ConexionSql"));
            });
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IRolRepository, RolRepository>();


            return services;
        }

        private static void ConfigureJwtOptions(IServiceCollection services) =>
            services.ConfigureOptions<JwtOptionsSetup>();
    }
}