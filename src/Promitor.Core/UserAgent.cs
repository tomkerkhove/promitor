using GuardNet;

namespace Promitor.Core
{
    public static class UserAgent
    {
        private const string UserAgentFormat = "App/Promitor Agent/{0} Version/{1}";

        /// <summary>
        ///     Generates a user-agent section
        /// </summary>
        /// <param name="agentName">Name of the Promitor agent</param>
        /// <param name="version">Current version</param>
        public static string Generate(string agentName, string version)
        {
            Guard.NotNullOrWhitespace(agentName, nameof(agentName));
            Guard.NotNullOrWhitespace(version, nameof(version));

            return string.Format(UserAgentFormat, agentName, version);
        }
    }
}