namespace Promitor.Agents.Core.Validation.Interfaces
{
    public interface IValidationStep
    {
        string ComponentName { get; }
        ValidationResult Run();
    }
}