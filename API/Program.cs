using Infrastructure;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.DefaultData;
using Microsoft.EntityFrameworkCore;
using Presentation;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Agregar los servicios 

builder.Services.AddHttpContextAccessor();
builder.Services.AddInfraestructureServices(builder.Configuration);
builder.Services.AddPresentationServices();

var presentationAssembly = typeof(PresentationServiceRegistration).Assembly;
builder.Services.AddControllers().AddApplicationPart(presentationAssembly);

#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UsePresentationMiddleware();
app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    var loggerFactory = service.GetRequiredService<ILoggerFactory>();

    try
    {
        var context = service.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();
        await ApplicationDbContextSeedData.LoadDataAsync(context, loggerFactory);
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "Error en migration");

    }
};

app.Run();
