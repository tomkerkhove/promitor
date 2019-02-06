using System.Collections.Generic;
using System.Runtime.Serialization;
using GuardNet;
using Microsoft.Extensions.Logging;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization
{
    internal abstract class Deserializer<TObject>
    {
        protected ILogger Logger { get; }

        internal Deserializer(ILogger logger)
        {
            Guard.NotNull(logger, nameof(logger));

            Logger = logger;
        }

        internal abstract TObject Deserialize(YamlMappingNode node);

        internal List<TObject> Deserialize(YamlSequenceNode nodes)
        {
            Guard.NotNull(nodes, nameof(nodes));

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