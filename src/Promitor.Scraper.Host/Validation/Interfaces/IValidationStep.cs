namespace Promitor.Scraper.Host.Validation.Interfaces
{
    public interface IValidationStep
    {
        string ComponentName { get; }
        ValidationResult Run();
    }
}