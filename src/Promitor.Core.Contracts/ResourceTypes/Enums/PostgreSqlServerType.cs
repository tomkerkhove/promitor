namespace Promitor.Core.Contracts.ResourceTypes.Enums
{
    public enum PostgreSqlServerType
    {
        // Azure Database for PostgreSQL Single Server
        // See: https://docs.microsoft.com/en-us/azure/postgresql/single-server/
        Single,

        // Azure Database for PostgreSQL - Flexible Server
        // See: https://docs.microsoft.com/en-us/azure/postgresql/flexible-server/
        Flexible,

        // Azure Database for PostgreSQL - Hyperscale (Citus) 
        // See: https://docs.microsoft.com/en-us/azure/postgresql/hyperscale/
        Hyperscale,

        // Azure Arc-enabled PostgreSQL Hyperscale server group
        // See: https://docs.microsoft.com/en-us/azure/azure-arc/data/what-is-azure-arc-enabled-postgres-hyperscale
        Arc
    }
}
