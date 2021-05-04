namespace Promitor.Core
{
    public static class Defaults
    {
        public static class AppServices
        {
            public static string SlotName { get; } = "production";
        }

        public static class MetricDefaults
        {
            public static int Limit => 10000;
        }
    }
}
