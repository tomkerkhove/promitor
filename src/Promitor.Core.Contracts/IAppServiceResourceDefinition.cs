namespace Promitor.Core.Contracts
{
    public interface IAppServiceResourceDefinition : IAzureResourceDefinition
    {
        public string SlotName { get; }
    }
}