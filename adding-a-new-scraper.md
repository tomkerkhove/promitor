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

<!-- markdownlint-disable MD013 -->
1. Add your new scraping type to the `Promitor.Core.Scraping.Configuration.Model.ResourceType`.
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
6. Update the `Promitor.Core.Scraping.Configuration.Serialization.v1.Mapping.V1MappingProfile` to handle your new resource type by mapping the `<New-Type>ResourceV1` to `<New-Type>ResourceDefinition` and annotating how to map it to `IAzureResourceDefinition`.
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
3. Provide a unit test for every validation rule that was added in `.\src\Promitor.Tests.Unit\Validation\Metrics\ResourceTypes`

### Scraping

We'll add a new scraper that pulls the metrics from Azure Monitor:

1. Implement a scraper, that inherits from `AzureMonitorScraper<TResourceDefinition>`, which will specify what resource to scrape with Azure Monitor.
2. Hook your new scraper in our `MetricScraperFactory` which determines what scraper
  to use for the passed configuration.

### Resource Discovery

We'll add dynamic resource discovery support by using Azure Resource Graph:

1. Implement a new discovery query that [create an Azure Resource Graph query](https://docs.microsoft.com/en-us/azure/governance/resource-graph/concepts/query-language).It should inherits from `ResourceDiscoveryQuery` and be located in `.\src\Promitor.Agents.ResourceDiscovery\Graph\ResourceTypes`
2. Support the new resource type in `ResourceDiscoveryFactory`
3. Add discovery support badge in scraper documentation page - `![Resource Discovery Support Badge](https://img.shields.io/badge/Support%20for%20Resource%20Discovery-Yes-green.svg)`
4. Add scraper to supported scrapers on resource discovery configuration documentation page `docs/configuration/v2.x/resource-discovery.md` in alphabetical order.

<!-- markdownlint-enable -->

------------------------

:memo: _Currently we don't have integration tests_

------------------------

## Documentation

Features are great to have but without clear & well-written documentation they are
somewhat useless.

Please provide documentation on the following:

1. What Azure service it supports and how to use it.
2. What fields need to be configured and what they are for.
3. An example configuration.

This should be provided in a new file under `docs\configuration\v2.x\metrics` and be listed
under the supported providers on `docs/configuration/v2.x/metrics/index.md` in alphabetical order.

## See It In Action

Now that you are done, make sure you run Promitor locally so verify that it generates the correct metrics!

When opening the pull request (PR), feel free to copy the generated Prometheus metrics for review.

Learn how to run it in our [development guide](development-guide.md#running-promitor).
