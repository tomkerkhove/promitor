using System.IO;
using System.Linq;
using YamlDotNet.RepresentationModel;

namespace Promitor.Tests.Unit.Serialization
{
    public class YamlUtils
    {
        public static YamlMappingNode CreateYamlNode(string yamlText)
        {
            var reader = new StringReader(yamlText);
            var stream = new YamlStream();
            stream.Load(reader);

            return (YamlMappingNode)stream.Documents.First().RootNode;
        }
    }
}
