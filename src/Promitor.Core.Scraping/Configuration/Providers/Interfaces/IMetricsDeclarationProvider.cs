using Promitor.Core.Scraping.Configuration.Model;

namespace Promitor.Core.Scraping.Configuration.Providers.Interfaces
{
    public interface IMetricsDeclarationProvider
    {
        /// <summary>
        ///     Gets the configured metrics declaration
        /// </summary>
        /// <param name="applyDefaults"><c>true</c> if the provider should apply default values from top-level 
        /// configuration elements to metrics where those values aren't specified. <c>false</c> otherwise</param>
        MetricsDeclaration Get(bool applyDefaults = false);

        /// <summary>
        ///     Gets the serialized metrics declaration
        /// </summary>
        string ReadRawDeclaration();
    }
}