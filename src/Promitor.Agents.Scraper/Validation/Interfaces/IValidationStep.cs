namespace Promitor.Agents.Scraper.Validation.Interfaces
{
    public interface IValidationStep
    {
        string ComponentName { get; }
        ValidationResult Run();
    }
}