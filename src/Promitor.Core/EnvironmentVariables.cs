namespace Promitor.Core
{
    public class EnvironmentVariables
    {
        public class API
        {
            public const string Prefix = "RUNTIME_API_PREFIX";
        }
        public class Configuration
        {
            public const string Folder = "PROMITOR_CONFIG_FOLDER";
        }

        public class Authentication
        {
            public const string ApplicationId = "AUTH_APPID";
            public const string ApplicationKey = "AUTH_APPKEY";
        }

        public class Integrations
        {
            public class AtlassianStatuspage
            {
                public const string ApiKey = "ATLASSIAN_STATUSPAGE_APIKEY";
            }
        }
    }
}