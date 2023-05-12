namespace Application.Features.Auth.Command.TokenValidation
{
    using Application.Helper;
    using FluentValidation;

    public class TokenValidationValidation : AbstractValidator<TokenValidationCommand>
    {
        public TokenValidationValidation()
        {
            RuleFor(x => x.Token)
                .NotEmpty()
                .WithMessage(ValidationMessages.isRequiredMessage)
                .WithName("Token");

        }
    }
}
