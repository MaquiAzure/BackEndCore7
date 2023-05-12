namespace Presentation.Controllers
{
    using Application.Features.Auth.Command.Login;
    using Application.Features.Auth.Command.Register;
    using Application.Features.Auth.Command.TokenValidation;
    using Application.Models;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Presentation.Controllers.Common;
    using System.Net;

    public class AuthController : BaseApiController
    {
        public AuthController(IMediator mediator) : base(mediator) { }

        [HttpPost("Login", Name = "Login")]
        [ProducesResponseType(typeof(AuthenticationResult), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<AuthenticationResult>> Login([FromBody] LoginCommand command)
        {
            return await _mediator.Send(command);
        }
        [Authorize(AuthenticationSchemes = "CustomAuthentication")]
        [HttpPost("Register", Name = "Register")]
        [ProducesResponseType(typeof(AuthenticationResult), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<AuthenticationResult>> Register([FromBody] RegisterCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPost("ValidateToken", Name = "ValidateToken")]
        [ProducesResponseType(typeof(TokenResult), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<TokenResult>> ValidateToken([FromBody] TokenValidationCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
