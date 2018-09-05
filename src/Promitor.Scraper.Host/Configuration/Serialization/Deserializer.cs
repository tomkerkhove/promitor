using System.Collections.Generic;
using System.Runtime.Serialization;
using YamlDotNet.RepresentationModel;

namespace Promitor.Scraper.Host.Configuration.Serialization
{
    internal abstract class Deserializer<TObject>
    {
        internal abstract TObject Deserialize(YamlMappingNode node);

        internal List<TObject> Deserialize(YamlSequenceNode nodes)
        {
            Guard.Guard.NotNull(nodes, nameof(nodes));

            var deserializedObjects = new List<TObject>();
            foreach (var item in nodes)
            {
                var metricNode = item as YamlMappingNode;
                if (metricNode == null)
                {
                    throw new SerializationException($"Failed parsing metrics because we couldn't cast an item to {nameof(YamlMappingNode)}");
                }

                var deserializedObject = Deserialize(metricNode);
                deserializedObjects.Add(deserializedObject);
            }

            return deserializedObjects;
        }
    }
}