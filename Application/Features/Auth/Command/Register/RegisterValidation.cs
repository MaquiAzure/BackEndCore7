namespace Application.Features.Auth.Command.Register
{
    using Application.Helper;
    using FluentValidation;
    public class RegisterValidation : AbstractValidator<RegisterCommand>
    {
        public RegisterValidation()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage(ValidationMessages.RequiredMessage)
                .EmailAddress()
                .WithMessage(ValidationMessages.EmailValidMessage)
                .WithName("Email");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(ValidationMessages.RequiredMessage)
                .Must(ValidatorHelper.ValidatePassword)
                .WithMessage(ValidationMessages.PasswordInvalidMessage)
                .WithName("Password");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage(ValidationMessages.RequiredMessage)
                .Equal(x => x.Password).WithMessage(ValidationMessages.isRequiredMessage)
                .WithName("ConfirmPassword");
        }
    }
}
