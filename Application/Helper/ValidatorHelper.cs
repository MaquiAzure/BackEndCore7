namespace Application.Helper
{
    public class ValidatorHelper
    {
        public static bool ValidatePassword(string? password)
        {
            if (password == null)
            {
                return true;
            }


            return password.Any(char.IsUpper) &&
                        password.Any(char.IsLower) &&
                        password.Any(char.IsDigit) &&
                        password.Any(ch => !char.IsLetterOrDigit(ch)) &&
                        password.Length >= 8;
        }
    }
}
