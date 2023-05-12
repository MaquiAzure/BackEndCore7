namespace Application.Features.Auth.Command.Register
{
    using Application.Contracts.Repositories.Base;
    using Application.Contracts.Service;
    using Application.Exception;
    using Application.Models;
    using AutoMapper;
    using Domain.Entities;
    using MediatR;
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthenticationResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public RegisterCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<AuthenticationResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            await verificarEmailNoTomado(request.Email);
            var usuario = await createUser(request.Email, request.Password);
            await addUserRole(usuario, "Role_Usuario");
            await confirmTransaction();
            return await CreateAuthenticationResult(usuario!);
        }
        private async Task verificarEmailNoTomado(string email)
        {
            var usuario = await _unitOfWork.UsuarioRepository.FindByEmailAsync(email);
            if (usuario != null)
            {
                throw new AuthException("Error en el registro de Usuario", $"Existe un usuario con el email: '{email}'");
            }
        }
        private async Task<Usuario> createUser(string email, string password)
        {
            var usuario = new Usuario
            {
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            };
            _unitOfWork.Repository<Usuario>().AddEntity(usuario);

            return usuario;
        }
        private async Task addUserRole(Usuario usuario, string roleName)
        {
            await _unitOfWork.UsuarioRepository.AddToRoleAsync(usuario, roleName);
        }
        private async Task confirmTransaction()
        {
            var result = await _unitOfWork.Complete();
            if (result <= 0)
            {
                //_logger.LogError("No se inserto el Usuario");
                throw new AuthException("Error en el registro de Usuario", $"No se inserto el Usuario");
            }
            //_logger.LogError($"El Usuario con Id {usuario.Id} se registrado correctamente");
        }
        private async Task<AuthenticationResult> CreateAuthenticationResult(Usuario user)
        {
            var token = await _tokenService.GenerateToken(user.Id, user.Email);
            return new AuthenticationResult { IsAuthenticated = true, AccessToken = token };
        }
    }
}
