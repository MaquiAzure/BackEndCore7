namespace Presentation.Errors
{
    public class AuthorizationException : ApplicationException
    {
        public AuthorizationException(string messague) : base(messague)
        {
        }
    }
}
