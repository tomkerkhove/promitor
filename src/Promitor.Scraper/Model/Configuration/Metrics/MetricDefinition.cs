namespace Promitor.Scraper.Model.Configuration.Metrics
{
    public class MetricDefinition
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public virtual ResourceType ResourceType { get; set; }
    }
}