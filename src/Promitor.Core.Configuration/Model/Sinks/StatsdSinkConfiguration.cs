namespace Promitor.Core.Configuration.Model.Sinks
{
    public class StatsdSinkConfiguration
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string MetricPrefix { get; set; }
    }
}