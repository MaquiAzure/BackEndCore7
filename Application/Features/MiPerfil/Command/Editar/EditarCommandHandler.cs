namespace Application.Features.Usuario.Command.Editar
{
    using Application.Contracts.Repositories.Base;
    using Application.Contracts.Service;
    using MediatR;
    public class EditarCommandHandler : IRequestHandler<EditarCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;

        public EditarCommandHandler(IUnitOfWork unitOfWork, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }

        public async Task<int> Handle(EditarCommand request, CancellationToken cancellationToken)
        {
            //var usuario = await verificarEmailExiste(request.Email);
            //await validarContraseña(usuario, request.Password);
            //return await CreateAuthenticationResult(usuario!);
            return 1;
        }
        //private async Task<Usuario> verificarEmailExiste(string email)
        //{
        //    var usuario = await _unitOfWork.UsuarioRepository.FindByEmailAsync(email);
        //    if (usuario == null)
        //    {
        //        throw new AuthException("Credenciales son incorrectas", $"El usuario con email {email} no existe");
        //    }
        //    return usuario;
        //}
        //private async Task validarContraseña(Usuario usuario, string password)
        //{
        //    bool passwordIsValid = BCrypt.Net.BCrypt.Verify(password, usuario.PasswordHash);
        //    if (!passwordIsValid)
        //    {
        //        throw new AuthException("Credenciales son incorrectas", $"El usuario {usuario.Email} no tiene la clave ingresada");
        //    }
        //}
        //private async Task<AuthenticationResult> CreateAuthenticationResult(Usuario user)
        //{
        //    var token = await _tokenService.GenerateToken(user.Id, user.Email);
        //    return new AuthenticationResult { IsAuthenticated = true, AccessToken = token };
        //}
    }

}
