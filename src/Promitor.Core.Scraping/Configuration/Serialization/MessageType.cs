namespace Promitor.Core.Scraping.Configuration.Serialization
{
    /// <summary>
    /// Defines the type of message reported during deserialization.
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// The message is a warning, and doesn't prevent Promitor from functioning.
        /// </summary>
        Warning,

        /// <summary>
        /// The message is an error which means that Promitor cannot use the configuration.
        /// </summary>
        Error
    }
}