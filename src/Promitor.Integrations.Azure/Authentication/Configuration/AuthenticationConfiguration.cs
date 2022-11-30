namespace Promitor.Integrations.Azure.Authentication.Configuration
{
    public class AuthenticationConfiguration
    {
        public AuthenticationMode Mode { get; set; } = AuthenticationMode.ServicePrincipal;
        public string IdentityId { get; set; }
        public string SecretFilePath { get; set; }
        public string SecretFileName { get; set; }
    }
}
