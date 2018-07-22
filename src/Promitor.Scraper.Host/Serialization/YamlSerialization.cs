using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Promitor.Scraper.Host.Serialization
{
    public static class YamlSerialization
    {
        private static readonly INamingConvention namingConvention = new CamelCaseNamingConvention();

        public static Deserializer CreateDeserializer()
        {
            var builder = new DeserializerBuilder();
            builder.IgnoreUnmatchedProperties();
            builder.WithNamingConvention(namingConvention);

            return builder.Build();
        }

        public static Serializer CreateSerializer()
        {
            var builder = new SerializerBuilder();
            builder.WithNamingConvention(namingConvention);

            return builder.Build();
        }
    }
}