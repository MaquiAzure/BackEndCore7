namespace Application.Contracts.Service
{
    using Application.Models;
    public interface ITokenService
    {
        Task<string> GenerateToken(Guid usernameId, string userEmail);
        Task<TokenResult> ValidateToken(string token);
    }
}
