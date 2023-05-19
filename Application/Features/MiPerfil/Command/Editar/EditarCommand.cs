namespace Application.Features.Usuario.Command.Editar
{
    using MediatR;
    public sealed record EditarCommand(Guid IdUsuarioRoute,string Nombres, string Apellidos) : IRequest<Unit>;
}
