namespace Promitor.Core
{
    public static class RuntimeMetricNames
    {
        public static string RateLimitingForArm => "promitor_ratelimit_arm";
        public static string ArmThrottled => "promitor_ratelimit_arm_throttled";
        public static string RateLimitingForResourceGraph => "promitor_ratelimit_resource_graph_remaining";
        public static string ResourceGraphThrottled => "promitor_ratelimit_resource_graph_throttled";
        public static string ScrapeSuccessful => "promitor_scrape_success";
        public static string ScrapeError => "promitor_scrape_error";
    }
}