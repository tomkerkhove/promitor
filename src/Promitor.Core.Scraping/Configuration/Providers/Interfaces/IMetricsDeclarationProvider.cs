using Promitor.Core.Scraping.Configuration.Model;

namespace Promitor.Core.Scraping.Configuration.Providers.Interfaces
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
        string ReadRawDeclaration();
    }
}