using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using AutoMapper;
using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Serialization.Enum;
using Promitor.Core.Serialization.Yaml;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization
{
    public class ConfigurationSerializer
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IDeserializer<MetricsDeclarationV1> _v1Deserializer;

        public ConfigurationSerializer(ILogger<ConfigurationSerializer> logger, IMapper mapper, IDeserializer<MetricsDeclarationV1> v1Deserializer)
        {
            _logger = logger;
            _mapper = mapper;
            _v1Deserializer = v1Deserializer;
        }

        public MetricsDeclaration Deserialize(string rawMetricsDeclaration, IErrorReporter errorReporter)
        {
            Guard.NotNullOrWhitespace(rawMetricsDeclaration, nameof(rawMetricsDeclaration));
            Guard.NotNull(errorReporter, nameof(errorReporter));

            var input = new StringReader(rawMetricsDeclaration);
            try
            {
                var metricsDeclarationYamlStream = new YamlStream();
                metricsDeclarationYamlStream.Load(input);

                var metricsDeclaration = InterpretYamlStream(metricsDeclarationYamlStream, errorReporter);

                return metricsDeclaration;
            }
            catch (Exception ex)
            {
                throw new SerializationException("Unable to deserialize the configured metrics declaration. Please verify that it is a well-formed YAML specification.", ex);
            }
        }

        private MetricsDeclaration InterpretYamlStream(YamlStream metricsDeclarationYamlStream, IErrorReporter errorReporter)
        {
            var document = metricsDeclarationYamlStream.Documents.First();
            var rootNode = (YamlMappingNode)document.RootNode;

            var specVersion = DetermineDeclarationSpecVersion(rootNode);
            _logger.LogTrace("Metrics declaration is using spec version {SpecVersion}", specVersion);

            switch (specVersion)
            {
                case SpecVersion.v1:
                    var v1Config = _v1Deserializer.Deserialize(rootNode, errorReporter);

                    return _mapper.Map<MetricsDeclaration>(v1Config);
                default:
                    throw new Exception($"Unable to interpret YAML stream for spec version '{specVersion}'");
            }
        }

        private SpecVersion DetermineDeclarationSpecVersion(YamlMappingNode mappingNode)
        {
            if (mappingNode.Children.ContainsKey("version") == false)
            {
                throw new Exception("No version was specified in the metric declaration");
            }

            var rawSpecVersion = mappingNode.Children[new YamlScalarNode("version")].ToString();

            if (Enum.TryParse(typeof(SpecVersion), rawSpecVersion, true, out var specVersion) == false)
            {
                throw new Exception($"Unable to determine version '{rawSpecVersion}' that was specified in the metric declaration");
            }

            // ReSharper disable once PossibleNullReferenceException
            return (SpecVersion)specVersion;
        }

        public string Serialize(object metricsDeclaration)
        {
            Guard.NotNull(metricsDeclaration, nameof(metricsDeclaration));

            var serializer = YamlSerialization.CreateSerializer();
            var rawMetricsDeclaration = serializer.Serialize(metricsDeclaration);
            return rawMetricsDeclaration;
        }
    }
}