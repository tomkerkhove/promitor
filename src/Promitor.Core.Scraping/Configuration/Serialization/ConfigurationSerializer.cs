﻿using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using AutoMapper;
using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Serialization.Enum;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model;
using Promitor.Core.Serialization.Yaml;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization
{
    public class ConfigurationSerializer
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IDeserializer<MetricsDeclarationV2> _v2Deserializer;

        public ConfigurationSerializer(ILogger logger, IMapper mapper, IDeserializer<MetricsDeclarationV2> v2Deserializer)
        {
            _logger = logger;
            _mapper = mapper;
            _v2Deserializer = v2Deserializer;
        }

        public MetricsDeclaration Deserialize(string rawMetricsDeclaration)
        {
            Guard.NotNullOrWhitespace(rawMetricsDeclaration, nameof(rawMetricsDeclaration));

            var input = new StringReader(rawMetricsDeclaration);
            try
            {
                var metricsDeclarationYamlStream = new YamlStream();
                metricsDeclarationYamlStream.Load(input);

                var metricsDeclaration = InterpretYamlStream(metricsDeclarationYamlStream);

                return metricsDeclaration;
            }
            catch (Exception ex)
            {
                throw new SerializationException("Unable to deserialize the configured metrics declaration. Please verify that it is a well-formed YAML specification.", ex);
            }
        }

        private MetricsDeclaration InterpretYamlStream(YamlStream metricsDeclarationYamlStream)
        {
            var document = metricsDeclarationYamlStream.Documents.First();
            var rootNode = (YamlMappingNode)document.RootNode;

            var specVersion = DetermineDeclarationSpecVersion(rootNode);
            _logger.LogInformation("Metrics declaration is using spec version {SpecVersion}", specVersion);

            switch (specVersion)
            {
                case SpecVersion.v1:
                    var v2Config = _v2Deserializer.Deserialize(rootNode);

                    return _mapper.Map<MetricsDeclaration>(v2Config);
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

            var rawSpecVersion = mappingNode.Children[new YamlScalarNode("version")]?.ToString();

            if (System.Enum.TryParse(typeof(SpecVersion), rawSpecVersion, true, out var specVersion) == false)
            {
                throw new Exception($"Unable to determine version '{rawSpecVersion}' that was specified in the metric declaration");
            }

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