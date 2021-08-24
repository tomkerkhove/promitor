namespace Promitor.Core.Contracts.ResourceTypes
{
    public class DataFactoryResourceDefinition : AzureResourceDefinition
    {
        public DataFactoryResourceDefinition(string subscriptionId, string resourceGroupName, string factoryName)
            : base(ResourceType.DataFactory, subscriptionId, resourceGroupName, factoryName)
        {
            FactoryName = factoryName;
        }

        public string FactoryName { get; }
    }
}