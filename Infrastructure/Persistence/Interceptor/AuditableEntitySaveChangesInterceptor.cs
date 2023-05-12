namespace Infrastructure.Persistence.Interceptor
{
    using Domain.Entities.Common;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.EntityFrameworkCore;
    using System.Security.Claims;
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using System.Text.Json;
    using System.Threading;

    public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditableEntitySaveChangesInterceptor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);
            base.SavingChangesAsync(eventData, result);
            OnAfterSaveChanges(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public async override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);
            await base.SavingChangesAsync(eventData, result, cancellationToken);
            OnAfterSaveChanges(eventData.Context);
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public void UpdateEntities(DbContext? context)
        {
            if (context == null) return;

            foreach (var entry in context.ChangeTracker.Entries<BaseDomainModel>())
            {
                var httpContext = _httpContextAccessor.HttpContext;
                string userConnected = httpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "system";

                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Id = Guid.NewGuid();
                        entry.Entity.isActive = true;
                        entry.Entity.CreatedDate = DateTime.Now;
                        entry.Entity.CreatedBy = userConnected;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LasModifiedDate = DateTime.Now;
                        entry.Entity.LastModifiedBy = userConnected;
                        break;

                }
            }
        }

        public void OnAfterSaveChanges(DbContext? context)
        {
            if (context == null) return;
            EntityEntry[] entries= context.ChangeTracker.Entries().ToArray(); 
            var httpContext = _httpContextAccessor.HttpContext;
            var user = httpContext?.User;
            string userConnected = user?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "system";
            foreach (var entry in entries)
            {
                if (entry.Entity is not Auditoria && entry.Entity is BaseDomainModel entity)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:

                            var auditoriaCreate = new Auditoria();
                            auditoriaCreate.Fecha = DateTime.Now;
                            auditoriaCreate.NombreTabla = entry.Metadata.GetTableName();
                            auditoriaCreate.IdOperacion = 1;
                            auditoriaCreate.OperacionRealizada = entry.State.ToString();
                            auditoriaCreate.IdRegistro = entity.Id.ToString();
                            auditoriaCreate.ValorActual = JsonSerializer.Serialize(entry.OriginalValues.ToObject());
                            auditoriaCreate.ValorActual = JsonSerializer.Serialize(entry.CurrentValues.ToObject()); 
                            auditoriaCreate.UserName = userConnected;

                            context.Set<Auditoria>().Add(auditoriaCreate);
                            break;
                        case EntityState.Modified:
                            var auditoriaModified = new Auditoria();
                            auditoriaModified.Fecha = DateTime.Now;
                            auditoriaModified.NombreTabla = entry.Metadata.GetTableName();
                            auditoriaModified.IdOperacion = 2;
                            auditoriaModified.OperacionRealizada = entry.State.ToString();
                            auditoriaModified.IdRegistro = entity.Id.ToString();
                            auditoriaModified.ValorOriginal = JsonSerializer.Serialize(entry.OriginalValues.ToObject()); 
                            auditoriaModified.ValorActual = JsonSerializer.Serialize(entry.CurrentValues.ToObject()); 
                            auditoriaModified.UserName = userConnected;

                            context.Set<Auditoria>().Add(auditoriaModified);
                            break;

                    }
                }
            }
        }


    }
}
