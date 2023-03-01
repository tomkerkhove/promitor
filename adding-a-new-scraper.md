# Adding a New Scraper

This guide walks you through the process of adding a new scraper type.

You can use our code tour by using the [CodeTour](https://marketplace.visualstudio.com/items?itemName=vsls-contrib.codetour)
 extension for VS Code and/or use it in [GitHub Codespaces](https://github.com/features/codespaces).

------------------------

:loudspeaker: _Before implementing a new scraping type, please open an issue to
discuss your scenario_

------------------------

## Implementing a Scaler

### Configuration

<!-- markdownlint-disable MD006 -->
<!-- markdownlint-disable MD007 -->
<!-- markdownlint-disable MD013 -->
<!-- markdownlint-disable MD029 -->
<!-- markdownlint-disable MD032 -->
1. Add your new scraping type to the `ResourceType` enum in `Promitor.Core.Contracts`.
2. Describe the resource for which you're scraping metrics by creating `<New-Type>ResourceDefinition`
  and inherit from
  `Promitor.Core.Scraping.Configuration.Model.Metrics.AzureResourceDefinition` -
  this class should go in `.\src\Promitor.Core.Contracts\ResourceTypes`.
3. Describe the resource configuration for which you're scraping metrics by creating
 `<New-Type>ResourceV1`
  and inherit from
   `Promitor.Core.Scraping.Configuration.Serialization.v1.Model.AzureResourceDefinitionV1` -
  this class should go in `.\src\Promitor.Core.Scraping\Configuration\Serialization\v1\Model\ResourceTypes`.
4. Create a new Deserializer in `.\src\Promitor.Core.Scraping\Configuration\Serialization\v1\Providers`.
  This must inherit from `ResourceDeserializer`.
5. Update `Promitor.Core.Scraping.Configuration.v1.Core.AzureResourceDeserializerFactory`
  to handle your new resource type by returning a new instance of the Deserializer
  you created in the previous step.
6. Update the `Promitor.Core.Scraping.Configuration.Serialization.v1.Mapping.V1MappingProfile` so that it:
  - Is able to map your new resource type by mapping the `<New-Type>ResourceV1` to `<New-Type>ResourceDefinition`
  - Annotate how to map it with `IAzureResourceDefinition` (include).
7. Provide a unit test in `.\src\Promitor.Tests.Unit\Serialization\v1\Providers`
  that tests the deserialization based on our sample. Your test class must inherit
  from `ResourceDeserializerTest` to ensure the inherited functionality is tested.

Going forward in this guide, `TResourceDefinition` will refer to your newly created
configuration type.

### Validation

For every scraper type we provide validation for the configuration so that Promitor
fails to start up.

This requires the following steps:

1. Create a new validator that implements `IMetricValidator`. This validator should
  reside in `.\src\Promitor.Agents.Scraper\Validation\MetricDefinitions\ResourceTypes`.
  You can look at the contents of `ServiceBusQueueMetricValidator` for an idea of
  the validation inputs, steps, and outputs typical of validator implementation.
2. Add construction and usage of this validator to `.\src\Promitor.Agents.Scraper\Validation\Factories\MetricValidatorFactory.cs`
  for the ResourceType you created in step #1 above.
3. Provide a unit test for every validation rule that was added in `.\src\Promitor.Tests.Unit\Validation\Scraper\Metrics\ResourceTypes`

### Scraping

We'll add a new scraper that pulls the metrics from Azure Monitor:

1. Implement a scraper, that inherits from `AzureMonitorScraper<TResourceDefinition>`, which will specify what resource to scrape with Azure Monitor.
    - You can find it in `.\src\Promitor.Core.Scraping\ResourceTypes`.
2. Hook your new scraper in our `MetricScraperFactory` which determines what scraper
  to use for the passed configuration.
    - You can find it in `.\src\Promitor.Core.Scraping\Factories`.

### Resource Discovery

We'll add dynamic resource discovery support by using Azure Resource Graph:

1. Implement a new discovery query that [create an Azure Resource Graph query](https://docs.microsoft.com/en-us/azure/governance/resource-graph/concepts/query-language).It should inherits from `ResourceDiscoveryQuery` and be located in `.\src\Promitor.Agents.ResourceDiscovery\Graph\ResourceTypes`
2. Support the new resource type in `ResourceDiscoveryFactory`

<!-- markdownlint-enable -->

------------------------

:memo: _Currently we don't have integration tests_

------------------------

## Writing Integration Tests

Every new scraper, should be automatically tested to ensure that it can be scraped.

To achieve this, the steps are fairly simple:

1. Provision a new test resource in our testing infrastructure ([GitHub](https://github.com/promitor/azure-infrastructure))
2. Define a new resource discovery group
    - You can find it on `config\promitor\resource-discovery\resource-discovery-declaration.yaml`
3. Define a new metric in the scraper configuration with a valid Azure Monitor metric for the service ([overview](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported)]
    - You can find it on `config\promitor\scraper\metrics.yaml`

Our testing infrastructure will pick up the new metric automatically and ensure that it is being reported!

## Documentation

Features are great to have but without clear & well-written documentation they are
somewhat useless.

The documentation for Promitor is hosted on [docs.promitor.io](https://docs.promitor.io) and is maintained in [promitor/docs](https://github.com/promitor/docs).

Please follow the instructions in the
[docs contribution guide](https://github.com/promitor/docs/blob/main/CONTRIBUTING.md#documenting-a-new-scraper)
for documenting a new scraper.

## Changelog

New scalers are a great additions and we should make sure that they are listed in our changelog.

Learn about our changelog in our [contribution guide](CONTRIBUTING.md#Changelog).

## See It In Action

Now that you are done, make sure you run Promitor locally so verify that it generates the correct metrics!

When opening the pull request (PR), feel free to copy the generated Prometheus metrics for review.

Learn how to run it in our [development guide](contributing.md#running-promitor).
