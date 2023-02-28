namespace Promitor.Integrations.Azure.Authentication
{
    public class AzureAuthenticationInfo
    {
        public AuthenticationMode Mode { get; set; }
        public string IdentityId { get; set; }
        public string Secret { get; set; }

        /// <summary>
        /// This method returns <see cref="IdentityId"/> if it is not null or whitespace, otherwise it returns <paramref name="defaultValue"/>.
        /// </summary>
        /// <param name="defaultValue">default value to return when <see cref="IdentityId"/> is null or whitespace.</param>
        /// <returns><see cref="IdentityId"/> or <paramref name="defaultValue"/>.</returns>
        public string GetIdentityIdOrDefault(string defaultValue = null)
        {
            return string.IsNullOrWhiteSpace(IdentityId) ? defaultValue : IdentityId;
        }
    }
}