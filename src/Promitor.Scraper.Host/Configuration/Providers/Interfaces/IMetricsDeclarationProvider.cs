using Promitor.Scraper.Host.Configuration.Model;

namespace Promitor.Scraper.Host.Configuration.Providers.Interfaces
{
    public interface IMetricsDeclarationProvider
    {
        /// <summary>
        ///     Gets the configured metrics declaration
        /// </summary>
        MetricsDeclaration Get();

        /// <summary>
        ///     Gets the serialized metrics declaration
        /// </summary>
        string GetSerializedDeclaration();
    }
}