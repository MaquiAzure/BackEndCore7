namespace Infrastructure.Services
{
    using Application.Contracts.Service;
    using Microsoft.AspNetCore.Http;
    using System.Security.Claims;
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        public string UserEmail => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
        public ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;
    }
}
