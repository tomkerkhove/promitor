using System.Collections.Generic;
using Cronos;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.FieldValidators
{
    /// <summary>
    /// Ensures that a field is a valid Cron expression.
    /// </summary>
    public class CronExpressionValidator : IFieldValidator
    {
        /// <inheritdoc />
        public void Validate(string value, KeyValuePair<YamlNode, YamlNode> fieldNodes, IErrorReporter errorReporter)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (!IsValidStandardExpression(value) && !IsValidExpressionWithSeconds(value))
                {
                    errorReporter.ReportError(fieldNodes.Value, $"'{value}' is not a valid value for '{fieldNodes.Key}'. The value must be a valid Cron expression.");
                }
            }
        }

        private static bool IsValidStandardExpression(string value)
        {
            try
            {
                CronExpression.Parse(value);
                return true;
            }
            catch (CronFormatException)
            {
                return false;
            }
        }

        private static bool IsValidExpressionWithSeconds(string value)
        {
            try
            {
                CronExpression.Parse(value, CronFormat.IncludeSeconds);
                return true;
            }
            catch (CronFormatException)
            {
                return false;
            }
        }
    }
}