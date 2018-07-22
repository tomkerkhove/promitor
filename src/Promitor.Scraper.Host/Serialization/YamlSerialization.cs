﻿using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Promitor.Scraper.Host.Serialization
{
    public static class YamlSerialization
    {
        private static readonly INamingConvention NamingConvention = new CamelCaseNamingConvention();

        public static Deserializer CreateDeserializer()
        {
            var builder = new DeserializerBuilder();
            builder.IgnoreUnmatchedProperties();
            builder.WithNamingConvention(NamingConvention);

            return builder.Build();
        }

        public static Serializer CreateSerializer()
        {
            var builder = new SerializerBuilder();
            builder.WithNamingConvention(NamingConvention);

            return builder.Build();
        }
    }
}