namespace Promitor.Agents.Core.Configuration.Server
{
    public class ServerConfiguration
    {
        public int HttpPort { get; set; } = Defaults.Server.HttpPort;

        public AuthenticationMode Authentication { get; set; } = Defaults.Server.Authentication;
    }
}
