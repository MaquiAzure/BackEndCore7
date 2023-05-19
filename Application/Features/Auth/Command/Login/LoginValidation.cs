namespace Application.Features.Auth.Command.Login
{
    using Application.Helper;
    using FluentValidation;
    public class LoginValidation : AbstractValidator<LoginCommand>
    {
        public LoginValidation()
        {
            RuleFor(x => x.Email)
                .NotNull()
                .WithMessage(ValidationMessages.RequiredMessage)
                .NotEmpty()
                .WithMessage(ValidationMessages.RequiredMessage)
                .EmailAddress()
                .WithMessage(ValidationMessages.EmailValidMessage)
                .WithName("Email");

            RuleFor(x => x.Password)
                .NotNull()
                .WithMessage(ValidationMessages.RequiredMessage)
                .NotEmpty()
                .WithMessage(ValidationMessages.RequiredMessage)
                .Must(ValidatorHelper.ValidatePassword)
                .WithMessage(ValidationMessages.PasswordInvalidMessage)
                .WithName("Password");
        }
    }
}
