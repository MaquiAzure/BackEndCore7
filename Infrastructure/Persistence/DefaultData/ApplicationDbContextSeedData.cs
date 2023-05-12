namespace Infrastructure.Persistence.DefaultData
{
    using Domain.Entities;
    using Infrastructure.Persistence.Contexts;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using System.Text.Json;
    public class ApplicationDbContextSeedData
    {
        public static async Task LoadDataAsync(ApplicationDbContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!context.Usuarios!.Any())
                {
                    var usuarioData = File.ReadAllText("../Infrastructure/Persistence/DefaultData/usuario.json");
                    var usuario = JsonSerializer.Deserialize<List<Usuario>>(usuarioData);
                    foreach (var user in usuario)
                    {
                        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
                    }
                    await context.Usuarios!.AddRangeAsync(usuario!);
                    await context.SaveChangesAsync();
                }

                if (!context.Roles!.Any())
                {
                    var rolData = File.ReadAllText("../Infrastructure/Persistence/DefaultData/rol.json");
                    var roles = JsonSerializer.Deserialize<List<Rol>>(rolData);
                    await context.Roles!.AddRangeAsync(roles!);
                    await context.SaveChangesAsync();
                }


                if (!context.UsuarioRoles!.Any())
                {
                    var usuarioList = await context.Usuarios!.ToListAsync();
                    var rolesList = await context.Roles!.ToListAsync();

                    var usuarioRolData = File.ReadAllText("../Infrastructure/Persistence/DefaultData/usuarioRol.json");
                    var usuarioRolList = JsonSerializer.Deserialize<List<UsuarioRol>>(usuarioRolData);
                    foreach (var usuarioRol in usuarioRolList)
                    {
                        var rol = rolesList.Where(x => x.Name == usuarioRol.Rol!.Name).FirstOrDefault();
                        if (rol != null)
                        {
                            usuarioRol.RolId = rol.Id;
                        }
                        var usuario = usuarioList.Where(x => x.Email == usuarioRol.Usuario!.Email).FirstOrDefault();
                        if (usuario != null)
                        {
                            usuarioRol.UsuarioId = usuario.Id;
                        }
                        usuarioRol.Rol = null;
                        usuarioRol.Usuario = null;

                    }
                    await context.UsuarioRoles!.AddRangeAsync(usuarioRolList!);
                    await context.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<ApplicationDbContextSeedData>();
                logger.LogError(ex.Message);
            }
        }

    }
}
