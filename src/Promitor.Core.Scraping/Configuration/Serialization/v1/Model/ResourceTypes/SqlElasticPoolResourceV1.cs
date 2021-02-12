namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    ///     Represents an Azure SQL Elastic Pool to scrape.
    /// </summary>
    public class SqlElasticPoolResourceV1 : SqlServerResourceV1
    {
        public SqlElasticPoolResourceV1()
        {
        }

        public SqlElasticPoolResourceV1(SqlServerResourceV1 sqlServerResource)
            : base(sqlServerResource.ServerName, sqlServerResource.ResourceGroupName)
        {
        }

        /// <summary>
        ///     The name of the elastic pool.
        /// </summary>
        public string PoolName { get; set; }
    }
}