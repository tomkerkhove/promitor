namespace Promitor.Scraper.Configuration
{
    public class EnvironmentVariables
    {
        public const string ConfigurationPath = "PROMITOR_CONFIGURATION_PATH";

        public class Authentication
        {
            public const string ApplicationId = "PROMITOR_AUTH_APPID";
            public const string ApplicationKey = "PROMITOR_AUTH_APPKEY";
        }

        public class Scraping
        {
            public const string CronSchedule = "PROMITOR_SCRAPE_SCHEDULE";
            public const string EndpointPath = "PROMITOR_SCRAPEENDPOINT_BASEPATH";
        }
    }
}