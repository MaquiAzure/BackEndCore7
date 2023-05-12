namespace Application.Exception
{
    public class AuthException : ApplicationException
    {
        public object Detail { get; set; }
        public AuthException(string messague, object detail) : base(messague)
        {
            Detail = detail;
        }
    }
}
