namespace Promitor.Integrations.Azure.Authentication
{
    public class AzureAuthenticationInfo
    {
        public AuthenticationMode Mode { get; set; }
        public string IdentityId { get; set; }
        public string Secret { get; set; }
    }
}