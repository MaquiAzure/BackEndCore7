namespace Application.Contracts.Service
{
    using System.Security.Claims;
    public interface ICurrentUserService
    {
        string UserId { get; }
        string UserEmail { get; }
        ClaimsPrincipal User { get; }
    }
}
