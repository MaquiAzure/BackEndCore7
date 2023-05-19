namespace Application.Helper
{
    public class ValidationMessages
    {
        public const string RequiredMessage = "El campo '{PropertyName}' es obligatorio.";
        public const string EmailValidMessage = "El campo '{PropertyName}' debe ser un correo electrónico valido para poder autenticarse.";
        public const string PasswordInvalidMessage = "la constraseña debe tener por lo mínimo una letra mayuscula, una letra minuscula, un caracter especial y una longitud de 8";
        public const string ConfirmPasswordIsNotEqualWithPassword = "El password y su confirmación deben ser iguales.";

    }
}
