namespace Promitor.Core.Scraping
{
    public static class Defaults
    {
        public static class MetricsConfiguration
        {
            public static string AbsolutePath { get; } = "/config/metrics-declaration.yaml";
        }
        public static class AppServices
        {
            public static string SlotName { get; } = "production";
        }
    }
}
