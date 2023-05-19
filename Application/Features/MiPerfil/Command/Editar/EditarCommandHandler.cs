namespace Application.Features.Usuario.Command.Editar
{
    using Application.Contracts.Repositories.Base;
    using Application.Contracts.Service;
    using MediatR;
    using Domain.Entities;
    using Application.Exception;
    using AutoMapper;
    public class EditarCommandHandler : IRequestHandler<EditarCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public EditarCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }
        public async Task<Unit> Handle(EditarCommand request, CancellationToken cancellationToken)
        {
            verificarAutorizacionParaCambiarPerfil(request.IdUsuarioRoute);
            var usuarioToUpdate = await obtenerUsuario(request.IdUsuarioRoute);
            _mapper.Map(request, usuarioToUpdate, typeof(EditarCommand), typeof(Usuario));
            _unitOfWork.UsuarioRepository.UpdateEntity(usuarioToUpdate);
            await _unitOfWork.Complete();
            return Unit.Value;
        }
        private void verificarAutorizacionParaCambiarPerfil(Guid idUsuarioToken)
        {
            var idUsuarioRoute = Guid.Parse(_currentUserService.UserId);
            if (idUsuarioToken!=idUsuarioRoute)
            {
                throw new ApplicationException("No está autorizado para cambiar este usuario");
            }
        }
        private async Task<Usuario> obtenerUsuario(Guid idUsuarioRoute)
        {
            var usuario = await _unitOfWork.UsuarioRepository.FindByIdUsuario(idUsuarioRoute);
            if (usuario == null)
            {
                throw new NotFoundException($"No se encontro el usuario con Id: '{idUsuarioRoute}'");
            }
            return usuario;
        }
    }
}
