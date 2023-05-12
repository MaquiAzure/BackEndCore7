namespace Infrastructure.Options
{
    public class JwtOptions
    {
        public string Issuer { get; init; } = default!;
        public string Audience { get; init; } = default!;
        public string SecretKey { get; init; } = default!;
        public double ExpirationTimeInHours { get; init; } = default!;
    }
}
