using System;
using System.Reflection;

namespace Promitor.Core
{
    public static class Version
    {
        /// <summary>
        ///     Get current version of agent
        /// </summary>
        public static string Get()
        {
            var executingAssembly = Assembly.GetEntryAssembly();
            if (executingAssembly == null)
            {
                throw new InvalidOperationException(
                    "Cannot enrich the log events with a 'Version' because the version of the current executing runtime couldn't be determined");
            }

            return executingAssembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
                   ?? executingAssembly.GetCustomAttribute<AssemblyVersionAttribute>()?.Version
                   ?? executingAssembly.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;
        }
    }
}