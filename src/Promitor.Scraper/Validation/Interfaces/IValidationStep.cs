namespace Promitor.Scraper.Validation.Interfaces
{
    public interface IValidationStep
    {
        string ComponentName { get; }
        ValidationResult Validate();
    }
}