using Application.Features.Auth.Command.Login;
using Application.Helper;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Usuario.Command.Editar
{
    public class EditarCommandValidation : AbstractValidator<EditarCommand>
    {
        public EditarCommandValidation()
        {
            RuleFor(x => x.Nombres)
                .NotNull()
                .WithMessage(ValidationMessages.isRequiredMessage)
                .NotEmpty()
                .WithMessage(ValidationMessages.isRequiredMessage)
                .WithName("Nombres");

            RuleFor(x => x.Apellidos)
                .NotNull()
                .WithMessage(ValidationMessages.isRequiredMessage)
                .NotEmpty()
                .WithMessage(ValidationMessages.isRequiredMessage)
                .WithName("Apellidos");
        }
    }

}
