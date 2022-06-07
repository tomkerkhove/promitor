namespace Promitor.Agents.Core.Configuration.Server
{
    public class ServerConfiguration
    {
        public int HttpPort { get; set; } = Defaults.Server.HttpPort;

        /// <summary>
        /// Upper limit on the number of concurrent threads between all possible scheduled scraping jobs,
        /// where 0 or negative is interpreted as unlimited. 
        /// </summary>
        public int MaxDegreeOfParallelism { get; set; } = Defaults.Server.MaxDegreeOfParallelism;
    }
}
