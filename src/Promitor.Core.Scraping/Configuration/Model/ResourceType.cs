namespace Promitor.Core.Scraping.Configuration.Model
{
    public enum ResourceType
    {
        NotSpecified = 0,
        ServiceBusQueue = 1,
        Generic = 2,
        StorageQueue = 3,
        ContainerInstance = 4,
        VirtualMachine = 5,
        ContainerRegistry = 6,
        NetworkInterface = 7,
        CosmosDb = 8,
        RedisCache = 9,
        PostgreSql = 10,
        SqlDatabase = 11,
        SqlManagedInstance = 12,
        VirtualMachineScaleSet = 13,
        AppPlan = 14,
        WebApp = 15,
        FunctionApp = 16,
        SqlServer = 17,
        BlobStorage = 18,
        FileStorage = 19,
        StorageAccount = 20,
        ApiManagement = 21,
        IoTHub = 22,
        DeviceProvisioningService = 23,
        KeyVault = 24
    }
}