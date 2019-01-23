using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Promitor.Core.Serialization.Yaml
{
    public static class YamlSerialization
    {
        private static readonly INamingConvention NamingConvention = new CamelCaseNamingConvention();
        
        public static ISerializer CreateSerializer()
        {
            var builder = new SerializerBuilder();
            builder.WithNamingConvention(NamingConvention);

            return builder.Build();
        }
    }
}