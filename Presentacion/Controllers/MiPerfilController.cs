namespace Presentation.Controllers
{
    using Application.Contracts.Service;
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
        [HttpPut("{Id}", Name = "EditarMiPerfil")]
        [ProducesResponseType(typeof(Unit), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Unit>> EditarMiPeril([FromBody] EditarCommand command, [FromRoute] string Id)
        {
            command = new EditarCommand(Id.ToUpper(), command.Nombres, command.Apellidos);
            return await _mediator.Send(command);
        }
    }
}
