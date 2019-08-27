using System.Collections.Generic;
using System.Runtime.Serialization;
using GuardNet;
using Microsoft.Extensions.Logging;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization
{
    public abstract class Deserializer<TObject> : IDeserializer<TObject>
    {
        protected ILogger Logger { get; }

        protected Deserializer(ILogger logger)
        {
            Guard.NotNull(logger, nameof(logger));

            Logger = logger;
        }

        public abstract TObject Deserialize(YamlMappingNode node);

        public List<TObject> Deserialize(YamlSequenceNode nodes)
        {
            Guard.NotNull(nodes, nameof(nodes));

            var deserializedObjects = new List<TObject>();
            foreach (var item in nodes)
            {
                if (!(item is YamlMappingNode metricNode))
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