using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization
{
    /// <summary>
    /// Represents a message reported during the deserialization process.
    /// </summary>
    public class DeserializationMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeserializationMessage"/> type.
        /// </summary>
        /// <param name="messageType">The type of message reported.</param>
        /// <param name="node">The node the message is associated with.</param>
        /// <param name="message">The message.</param>
        public DeserializationMessage(MessageType messageType, YamlNode node, string message)
        {
            MessageType = messageType;
            Node = node;
            Message = message;
        }

        /// <summary>
        /// Gets the type of message.
        /// </summary>
        public MessageType MessageType { get; }

        /// <summary>
        /// Gets the node the message has been reported against.
        /// </summary>
        public YamlNode Node { get; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        public string Message { get; }

        public string FormattedMessage => $"{MessageType} {Node.Start.Line}:{Node.Start.Column}: {Message}";

        public override string ToString()
        {
            return FormattedMessage;
        }
    }
}