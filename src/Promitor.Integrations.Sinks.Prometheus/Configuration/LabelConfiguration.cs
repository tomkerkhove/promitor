namespace Promitor.Integrations.Sinks.Prometheus.Configuration
{
    public class LabelConfiguration
    {
        public LabelTransformation Transformation { get; set; } = Defaults.Prometheus.LabelTransformation;
    }
}
