namespace Application.Features.Auth.Command.Login
{
    using Application.Models;
    using MediatR;
    public sealed record LoginCommand(string Email, string Password) : IRequest<AuthenticationResult>;
}
