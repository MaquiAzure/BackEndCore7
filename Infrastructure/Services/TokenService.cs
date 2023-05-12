namespace Infrastructure.Services
{
    using Application.Contracts.Repositories.Base;
    using Application.Contracts.Service;
    using Infrastructure.Options;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Application.Models;

    public class TokenService : ITokenService
    {
        private readonly JwtOptions _options;
        private readonly IUnitOfWork _unitOfWork;

        public TokenService(IOptions<JwtOptions> options, IUnitOfWork unitOfWork)
        {
            _options = options.Value;
            _unitOfWork = unitOfWork;
        }

        #region GenerateToken
        public async Task<string> GenerateToken(Guid usernameId, string userEmail)
        {
            var claims = await createClaims(usernameId, userEmail);

            var signingCredentials = CreateSigningCredentials();
            var tokenDescriptor = CreateJwtSecurityToken(claims, signingCredentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        private async Task<IEnumerable<Claim>> createClaims(Guid userId, string userEmail)
        {

            var roles = await _unitOfWork.UsuarioRepository.GetRolesByUserIDAsync(userId);

            var roleClaims = new List<Claim>();
            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("Role", role.Name));
            }
            var claims = new Claim[]
            {
                //new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, userEmail)
            }.Union(roleClaims);

            return claims;
        }
        private SigningCredentials CreateSigningCredentials()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
            return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        }
        private SecurityTokenDescriptor CreateJwtSecurityToken(IEnumerable<Claim> claims, SigningCredentials signingCredentials)
        {
            var claimsIdentity = new ClaimsIdentity(claims);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddHours(_options.ExpirationTimeInHours),
                SigningCredentials = signingCredentials,
                Issuer = _options.Issuer,
                Audience = _options.Audience

            };
            return tokenDescriptor;
        }
        #endregion

        #region Validate Token
        public async Task<TokenResult> ValidateToken(string token)
        {
            var firstValidation = ValidateSecretKeyAudienceAndIssuer(token);
            if (!firstValidation.IsValid) return firstValidation;

            var secondValid=ValidateExpiration(token);
            if (!secondValid.IsValid) return secondValid;

            return await ValidateEmail(token);

        }
        private TokenResult ValidateSecretKeyAudienceAndIssuer(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_options.SecretKey);

            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidIssuer = _options.Issuer,
                ValidAudience = _options.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero,
            };

            try
            {
                tokenHandler.ValidateToken(token, parameters, out var validatedToken);
                return new TokenResult { IsValid = true };
            }
            catch
            {
                return new TokenResult { IsValid = false, ErrorMessage = "Token no valido" };
            }
        }
        private TokenResult ValidateExpiration(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenData = tokenHandler.ReadJwtToken(token);

            var now = DateTime.UtcNow;
            var desde = tokenData.ValidFrom;
            var hasta = tokenData.ValidTo;

            var expirationTime = desde.AddHours(_options.ExpirationTimeInHours);
            var firstValid = now < hasta;
            var secondValid = expirationTime == hasta;
            if (!(firstValid && secondValid))
            {
                return new TokenResult { IsValid = false, ErrorMessage = "No existe el correo en el claim" };
            }
            return new TokenResult { IsValid = true };
        }
        private async Task<TokenResult> ValidateEmail(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenData = tokenHandler.ReadJwtToken(token);
            var emailClaim = tokenData.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email);
            if (emailClaim == null)
            {
                return new TokenResult { IsValid = true, ErrorMessage = "No existe el correo en el claim" };
            }
            var email = emailClaim.Value;
            var usuario = await _unitOfWork.UsuarioRepository.FindByEmailAsync(email);
            if (usuario == null)
            {
                return new TokenResult { IsValid = false, ErrorMessage="No existe el usuario" };
            }
            return new TokenResult{Email=email,UserId=usuario.Id.ToString(),IsValid=true};
        }
        #endregion
    }
}
