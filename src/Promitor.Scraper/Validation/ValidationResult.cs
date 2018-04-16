namespace Promitor.Scraper.Validation
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

        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        public string ComponentName { get; set; }

        public static ValidationResult Successful(string componentName)
        {
            return new ValidationResult(componentName, isSuccessful:true);
        }

        public static ValidationResult Failure(string componentName, string failureMessage)
        {
            return new ValidationResult(componentName, isSuccessful: false, failureMessage: failureMessage);
        }
    }
}