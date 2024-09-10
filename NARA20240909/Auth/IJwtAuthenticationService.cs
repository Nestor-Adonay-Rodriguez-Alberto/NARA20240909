namespace NARA20240909.Auth
{
    public interface IJwtAuthenticationService
    {
        // Generará un JWT:
        string Authenticate(string userName);
    }
}
