using System;

namespace Promitor.Agents.Core.Extensions
{
    /// <summary>
    /// Thrown if a required configuration file was not found.
    /// </summary>
    public class ConfigurationFileNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationFileNotFoundException" /> class.
        /// </summary>
        /// <param name="path">The full path to the file that was not found.</param>
        public ConfigurationFileNotFoundException(string path)
            : base($"A required configuration file was not found at '{path}'")
        {
            Path = path;
        }

        /// <summary>
        /// Gets the full path to the file that was not found.
        /// </summary>
        public string Path { get; }
    }
}