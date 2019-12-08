using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization
{
    public class ErrorReporter : IErrorReporter
    {
        public void ReportError(YamlNode node, string message)
        {
        }

        public void ReportWarning(YamlNode node, string message)
        {
        }
    }
}