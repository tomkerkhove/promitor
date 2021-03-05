using Microsoft.Extensions.Logging;
using Promitor.Agents.Core.Configuration.Authentication;

namespace Promitor.Agents.Core.Configuration
{
    public static class Defaults
    {
        public static class Authentication
        {
            public static AuthenticationMode Mode { get; } = AuthenticationMode.ServicePrincipal;
        }

        public static class Server
        {
            public static int HttpPort { get; } = 80;

        }
        
        public class Telemetry
        {
            public static LogLevel? DefaultVerbosity { get; set; } = LogLevel.Error;

            public class ContainerLogs
            {
                public static LogLevel? Verbosity { get; set; } = null;
                public static bool IsEnabled { get; set; } = true;
            }

            public class ApplicationInsights
            {
                public static LogLevel? Verbosity { get; set; } = null;
                public static bool IsEnabled { get; set; } = false;
            }
        }
    }
}
