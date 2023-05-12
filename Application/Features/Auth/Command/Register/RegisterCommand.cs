namespace Application.Features.Auth.Command.Register
{
    using Application.Models;
    using MediatR;
    public sealed record RegisterCommand(string Email, string Password, string ConfirmPassword) : IRequest<AuthenticationResult>;
}
