namespace Promitor.Core
{
    public class EnvironmentVariables
    {
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
        }
    }
}