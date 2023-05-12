namespace Application.Features.Usuario.Command.Editar
{
    using MediatR;
    public sealed record EditarCommand(string Nombres, string Apellidos) : IRequest<int>;
}
