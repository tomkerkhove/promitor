namespace Promitor.Agents.Core.Configuration.Authentication
{
    public enum AuthenticationMode
    {
        ServicePrincipal = 0,
        UserAssignedManagedIdentity = 1,
        SystemAssignedManagedIdentity = 2,
    }
}
