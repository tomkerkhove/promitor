namespace Promitor.Agents.Core
{
    /// <summary>
    /// The different statuses that the agent scraper can exit with.
    /// </summary>
    public enum ExitStatus
    {
        /// <summary>
        /// The application has run successfully.
        /// </summary>
        Success = 0,

        /// <summary>
        /// An unhandled exception was thrown during host startup. This probably
        /// indicates a bug in Promitor that should be reported.
        /// </summary>
        UnhandledException = 1,

        /// <summary>
        /// Validation failed, so Promitor can't start.
        /// </summary>
        ValidationFailed = 2,

        /// <summary>
        /// The configuration folder environment variable has not been set.
        /// </summary>
        ConfigurationFolderNotSpecified = 3,

        /// <summary>
        /// A required configuration file was not found.
        /// </summary>
        ConfigurationFileNotFound = 4
    }
}