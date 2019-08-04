namespace Promitor.Core.Configuration.Model.Metrics
{
    public class MetricsConfiguration
    {
        public string AbsolutePath { get; set; } = Defaults.MetricsConfiguration.AbsolutePath;
        public double? MetricUnavailableValue { get; set; } = Defaults.MetricsConfiguration.MetricUnavailableValue;
    }
}