using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.Interfaces
{
    internal interface IVersionedConfigurationSerializer
    {
        ILogger Logger { get; }
        MetricsDeclaration InterpretYamlStream(YamlMappingNode rootNode);
    }
}