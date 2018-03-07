namespace Promitor.Scraper.Validation
{
    public class ValidationResult
    {
        public static ValidationResult Successful = new ValidationResult {IsSuccessful = true};
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }

        public static ValidationResult Fail(string failureMessage)
        {
            return new ValidationResult
            {
                IsSuccessful = false,
                Message = failureMessage
            };
        }
    }
}