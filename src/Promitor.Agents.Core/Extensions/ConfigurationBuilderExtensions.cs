using System.IO;
using Microsoft.Extensions.Configuration;

namespace Promitor.Agents.Core.Extensions
{
    /// <summary>
    /// Extensions for <see cref="IConfigurationBuilder" />.
    /// </summary>
    public static class ConfigurationBuilderExtensions
    {
        /// <summary>
        /// Adds a required yaml file to the builder.
        /// </summary>
        /// <param name="builder">The configuration builder.</param>
        /// <param name="path">The full absolute path to the file.</param>
        /// <param name="reloadOnChange">
        /// Indicates whether the file will be reloaded if the contents change.
        /// </param>
        /// <exception cref="ConfigurationFileNotFoundException">
        /// Thrown if the specified configuration file was not found.
        /// </exception>
        public static IConfigurationBuilder AddRequiredYamlFile(
            this IConfigurationBuilder builder, string path, bool reloadOnChange)
        {
            if (!File.Exists(path))
            {
                throw new ConfigurationFileNotFoundException(path);
            }

            builder.AddYamlFile(path, optional: false, reloadOnChange: reloadOnChange);

            return builder;
        }
    }
}