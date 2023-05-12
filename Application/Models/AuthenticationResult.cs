namespace Application.Models
{
    public sealed class AuthenticationResult
    {
        public string AccessToken { get; set; } = string.Empty;
        public bool IsAuthenticated { get; set; } = false;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
