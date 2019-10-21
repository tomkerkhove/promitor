using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Promitor.Core.Serialization.Yaml
{
    public static class YamlSerialization
    {
        private static readonly INamingConvention namingConvention = CamelCaseNamingConvention.Instance;

        public static ISerializer CreateSerializer()
        {
            var builder = new SerializerBuilder();
            builder.WithNamingConvention(namingConvention);
            builder.ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitDefaults);

            return builder.Build();
        }
    }
}