namespace Promitor.Core
{
    public static class RuntimeMetricNames
    {
        public static string RateLimitingForArm => "promitor_ratelimit_arm";
        public static string ScrapeSuccessful => "promitor_scrape_success";
        public static string ScrapeError => "promitor_scrape_error";
    }
}