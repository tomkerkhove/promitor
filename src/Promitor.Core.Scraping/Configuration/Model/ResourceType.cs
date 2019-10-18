﻿namespace Promitor.Core.Scraping.Configuration.Model
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
        AzureSqlDatabase = 11
    }
}