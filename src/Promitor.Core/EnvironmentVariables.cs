namespace Promitor.Core
{
    public class EnvironmentVariables
    {
        public class Runtime
        {
            public const string HttpPort = "PROMITOR_HTTP_PORT";
        }

        public class Authentication
        {
            public const string ApplicationId = "PROMITOR_AUTH_APPID";
            public const string ApplicationKey = "PROMITOR_AUTH_APPKEY";
        }

        public class Configuration
        {
            public const string Path = "PROMITOR_CONFIGURATION_PATH";
        }

        public class Scraping
        {
            public const string CronSchedule = "PROMITOR_SCRAPE_SCHEDULE";
            public const string Path = "PROMITOR_SCRAPE_BASEPATH";
        }

        public class Telemetry
        {
            public const string InstrumentationKey = "PROMITOR_TELEMETRY_INSTRUMENTATIONKEY";
        }

        public class Logging
        {
            public const string MinimumLogLevel = "PROMITOR_LOGGING_MINIMUMLOGLEVEL";
        }
    }
}