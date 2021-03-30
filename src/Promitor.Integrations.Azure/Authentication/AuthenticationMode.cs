namespace Promitor.Integrations.Azure.Authentication
{
    public enum AuthenticationMode
    {
        ServicePrincipal = 0,
        UserAssignedManagedIdentity = 1,
        SystemAssignedManagedIdentity = 2,
    }
}
