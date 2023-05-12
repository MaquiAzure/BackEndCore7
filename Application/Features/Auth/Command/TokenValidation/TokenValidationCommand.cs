namespace Application.Features.Auth.Command.TokenValidation
{
    using Application.Models;
    using MediatR;
    public sealed record TokenValidationCommand(string Token) : IRequest<TokenResult>;
}
