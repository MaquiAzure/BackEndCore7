namespace Application.Features.Auth.Command.Login
{
    using Application.Contracts.Repositories.Base;
    using Application.Contracts.Service;
    using Application.Exception;
    using Application.Models;
    using Domain.Entities;
    using MediatR;
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthenticationResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;

        public LoginCommandHandler(IUnitOfWork unitOfWork, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }

        public async Task<AuthenticationResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var usuario = await verificarEmailExiste(request.Email);
            await validarContraseña(usuario, request.Password);
            return await CreateAuthenticationResult(usuario!);
        }
        private async Task<Usuario> verificarEmailExiste(string email)
        {
            var usuario = await _unitOfWork.UsuarioRepository.FindByEmailAsync(email);
            if (usuario == null)
            {
                throw new AuthException("Credenciales son incorrectas", $"El usuario con email {email} no existe");
            }
            return usuario;
        }
        private async Task validarContraseña(Usuario usuario, string password)
        {
            bool passwordIsValid = BCrypt.Net.BCrypt.Verify(password, usuario.PasswordHash);
            if (!passwordIsValid)
            {
                throw new AuthException("Credenciales son incorrectas", $"El usuario {usuario.Email} no tiene la clave ingresada");
            }
        }
        private async Task<AuthenticationResult> CreateAuthenticationResult(Usuario user)
        {
            var token = await _tokenService.GenerateToken(user.Id, user.Email);
            return new AuthenticationResult { IsAuthenticated = true, AccessToken = token };
        }
    }
}
