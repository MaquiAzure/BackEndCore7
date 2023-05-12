namespace Presentation
{
    using Application;
    using Application.Exception;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.DataProtection.KeyManagement;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using Presentation.Errors;
    using Presentation.Middleware;
    using System.Text;

    public static class PresentationServiceRegistration
    {
        public static IServiceCollection AddPresentationServices(this IServiceCollection services)
        {
            services.AddApplicationServices();
            //Controlar el error que está siendo producido como resultado de la validación del modelo (model binding) 
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                        );
                    return new BadRequestObjectResult(new CodeErrorException(400, "Error en el ingreso de datos", errors));
                };
            });
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //            .AddJwtBearer(options =>
            //            {
            //                options.TokenValidationParameters = new TokenValidationParameters
            //                {
            //                    ValidateIssuer = true,
            //                    ValidateAudience = true,
            //                    ValidateLifetime = true,
            //                    ValidateIssuerSigningKey = true,
            //                    ValidIssuer = configuration["Jwt:Issuer"],
            //                    ValidAudience = configuration["Jwt:Audience"],
            //                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"])),
            //                    ClockSkew = TimeSpan.Zero,
            //                };
            //            });
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddScheme<AuthenticationSchemeOptions, CustomAuthenticationHandler>("CustomAuthentication", options => { });

            return services;

        }
        public static IApplicationBuilder UsePresentationMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            return app;
        }
    }
}
