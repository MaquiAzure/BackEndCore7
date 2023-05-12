namespace Application.Features.Auth.Command.TokenValidation
{
    using Application.Contracts.Repositories.Base;
    using Application.Contracts.Service;
    using Application.Models;
    using AutoMapper;
    using Domain.Entities;
    using MediatR;
    public class TokenValidationHandler : IRequestHandler<TokenValidationCommand, TokenResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public TokenValidationHandler(IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<TokenResult> Handle(TokenValidationCommand request, CancellationToken cancellationToken)
        {
            
            return await _tokenService.ValidateToken(request.Token);
        }
        private async Task<AuthenticationResult> CreateAuthenticationResult(Usuario user)
        {
            var token = await _tokenService.GenerateToken(user.Id, user.Email);
            return new AuthenticationResult { IsAuthenticated = true, AccessToken = token };
        }
    }
}
