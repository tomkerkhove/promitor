using System;
using Microsoft.Extensions.Logging;

namespace Promitor.Agents.Core.Configuration
{
    public static class Defaults
    {
        public static class Server
        {
            public static int HttpPort { get; } = 80;

            /// <summary>
            /// Default upper limit on the number of concurrent threads between all possible scheduled concurrent scraping jobs,
            /// set to a reasonable load per CPU so as not to choke the system with processing overhead while attempting to
            /// communicate with cluster hosts and awaiting multiple outstanding API calls. 
            /// </summary>
            public static int MaxDegreeOfParallelism { get; } = Environment.ProcessorCount * 8;
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
