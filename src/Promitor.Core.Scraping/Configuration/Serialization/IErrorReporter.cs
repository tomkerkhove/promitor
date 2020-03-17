﻿using System.Collections.Generic;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization
{
    /// <summary>
    /// Used to report errors discovered during the deserialization process.
    /// </summary>
    public interface IErrorReporter
    {
        /// <summary>
        /// Contains the list of messages reported, in order of line number.
        /// </summary>
        IReadOnlyCollection<DeserializationMessage> Messages { get; }

        /// <summary>
        /// Indicates if any errors were reported.
        /// </summary>
        bool HasErrors { get; }

        /// <summary>
        /// Reports an error.
        /// </summary>
        /// <param name="node">The node containing the error / that should be highlighted to the user.</param>
        /// <param name="message">The error message.</param>
        void ReportError(YamlNode node, string message);

        /// <summary>
        /// Reports a warning (i.e. something that doesn't prevent Promitor from functioning).
        /// </summary>
        /// <param name="node">The node containing the error / that should be highlighted to the user.</param>
        /// <param name="message">The warning message.</param>
        void ReportWarning(YamlNode node, string message);
    }
}