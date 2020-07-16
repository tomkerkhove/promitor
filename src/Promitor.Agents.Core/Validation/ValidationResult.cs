using System.Collections.Generic;
using System.Text;

namespace Promitor.Agents.Core.Validation
{
    public class ValidationResult
    {
        private ValidationResult(string componentName, bool isSuccessful)
        {
            ComponentName = componentName;
            IsSuccessful = isSuccessful;
        }

        private ValidationResult(string componentName, bool isSuccessful, string failureMessage) : this(componentName,
            isSuccessful)
        {
            Message = failureMessage;
        }

        public string ComponentName { get; }

        public bool IsSuccessful { get; }
        public string Message { get; }

        public static ValidationResult Failure(string componentName, string failureMessage)
        {
            return new ValidationResult(componentName, isSuccessful: false, failureMessage: failureMessage);
        }

        public static ValidationResult Failure(string componentName, List<string> failureMessages)
        {
            var errorBuilder = new StringBuilder();
            failureMessages.ForEach(errorMessage => errorBuilder.AppendLine($"- {errorMessage}"));

            var failureMessage = errorBuilder.ToString();
            return new ValidationResult(componentName, isSuccessful: false, failureMessage: failureMessage);
        }

        public static ValidationResult Successful(string componentName)
        {
            return new ValidationResult(componentName, isSuccessful: true);
        }
    }
}