namespace Promitor.Core.Contracts.ResourceTypes
{
    public class DataFactoryResourceDefinition : AzureResourceDefinition
    {
        public DataFactoryResourceDefinition(string subscriptionId, string resourceGroupName, string factoryName, string pipelineName)
            : base(ResourceType.DataFactory, subscriptionId, resourceGroupName, $"{factoryName}-{pipelineName}")
        {
            FactoryName = factoryName;
            PipelineName = pipelineName;
        }

        public string FactoryName { get; }

        public string PipelineName { get; }
    }
}