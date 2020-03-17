using System.Collections.Generic;
using System.Linq;
using GuardNet;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization
{
    public class ErrorReporter : IErrorReporter
    {
        private readonly List<DeserializationMessage> _messages = new List<DeserializationMessage>();

        /// <inheritdoc />
        public IReadOnlyCollection<DeserializationMessage> Messages => _messages.OrderBy(m => m.Node.Start.Line).ToList();

        /// <inheritdoc />
        public bool HasErrors => Messages.Any(m => m.MessageType == MessageType.Error);

        /// <inheritdoc />
        public void ReportError(YamlNode node, string message)
        {
            Guard.NotNull(node, nameof(node));
            Guard.NotNullOrEmpty(message, nameof(message));

            _messages.Add(new DeserializationMessage(MessageType.Error, node, message));
        }

        /// <inheritdoc />
        public void ReportWarning(YamlNode node, string message)
        {
            Guard.NotNull(node, nameof(node));
            Guard.NotNullOrEmpty(message, nameof(message));

            _messages.Add(new DeserializationMessage(MessageType.Warning, node, message));
        }
    }
}