namespace Presentation.Middleware
{
    using Application.Contracts.Service;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Presentation.Errors;
    using System.Security.Claims;
    using System.Text.Encodings.Web;

    public class CustomAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly ITokenService _tokenservice;

        public CustomAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            ITokenService tokenservice)
            : base(options, logger, encoder, clock)
        {
            _tokenservice = tokenservice;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                throw new AuthorizationException("Missing authorization token");
            }

            var tokenValidationResult =await _tokenservice.ValidateToken(token);
            if (!tokenValidationResult.IsValid)
            {
                throw new AuthorizationException(tokenValidationResult.ErrorMessage);
            }

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, tokenValidationResult.UserId),
            new Claim(ClaimTypes.Email, tokenValidationResult.Email)
        };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
                IssuedUtc = DateTimeOffset.UtcNow
            };

            var ticket = new AuthenticationTicket(principal, authProperties, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}
