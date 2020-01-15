namespace Promitor.Core.Scraping.Configuration.Model.Metrics
{
    public interface IAppServiceResourceDefinition : IAzureResourceDefinition
    {
        public string SlotName { get; }
    }
}