namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    ///     Represents an Azure SQL Database to scrape.
    /// </summary>
    public class SqlDatabaseResourceV1 : SqlServerResourceV1
    {
        public SqlDatabaseResourceV1()
        {
        }

        public SqlDatabaseResourceV1(SqlServerResourceV1 sqlServerResource)
            : base(sqlServerResource.ServerName, sqlServerResource.ResourceGroupName)
        {
        }

        /// <summary>
        ///     The name of the SQL database.
        /// </summary>
        public string DatabaseName { get; set; }
    }
}