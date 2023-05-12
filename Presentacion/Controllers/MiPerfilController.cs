namespace Presentation.Controllers
{
    using Application.Features.Usuario.Command.Editar;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Presentation.Controllers.Common;
    using System.Net;
    public class MiPerfilController : BaseApiController
    {
        public MiPerfilController(IMediator mediator) : base(mediator) { }

        [Authorize(AuthenticationSchemes = "CustomAuthentication")]
        [HttpPut(Name = "EditarMiPerfil")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<int>> EditarMiPeril([FromBody] EditarCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
