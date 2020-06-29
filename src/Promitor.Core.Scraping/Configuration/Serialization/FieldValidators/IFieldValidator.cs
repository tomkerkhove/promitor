using System.Collections.Generic;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.FieldValidators
{
    /// <summary>
    /// Provides validation for a yaml field.
    /// </summary>
    public interface IFieldValidator
    {
        /// <summary>
        /// Validates a field, reporting any errors via the error reporter.
        /// </summary>
        /// <param name="value">The value of the field.</param>
        /// <param name="fieldNodes">A KeyValuePair where the Key is the field name, and the Value is its value.</param>
        /// <param name="errorReporter">The error reporter.</param>
        void Validate(string value, KeyValuePair<YamlNode, YamlNode> fieldNodes, IErrorReporter errorReporter);
    }
}