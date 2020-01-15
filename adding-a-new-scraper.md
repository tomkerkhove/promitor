# Adding a New Scraper

This guide walks you through the process of adding a new scraper type.

------------------------

:loudspeaker: _Before implementing a new scraping type, please open an issue to
discuss your scenario_

------------------------

## Configuration

<!-- markdownlint-disable MD013 -->
1. Add your new scraping type to the `Promitor.Core.Scraping.Configuration.Model.ResourceType`.
2. Describe the resource for which you're scraping metrics by creating `<New-Type>ResourceDefinition`
  and inherit from
  `Promitor.Core.Scraping.Configuration.Model.Metrics.AzureResourceDefinition` -
  this class should go in `.\src\Promitor.Core.Scraping\Configuration\Model\Metrics\ResourceTypes`.
3. Describe the resource configurationh for which you're scraping metrics by creating
 `<New-Type>ResourceV1`
  and inherit from
   `Promitor.Core.Scraping.Configuration.Serialization.v1.Model.AzureResourceDefinitionV1` -
  this class should go in `.\src\Promitor.Core.Scraping\Configuration\Serialization\v1\Model\ResourceTypes`.
4. Create a new Deserializer in `.\src\Promitor.Core.Scraping\Configuration\Serialization\v1\Providers`.
  This must inherit from `ResourceDeserializer`.
5. Update `Promitor.Core.Scraping.Configuration.v1.Core.AzureResourceDeserializerFactory`
  to handle your new resource type by returning a new instance of the Deserializer
  you created in the previous step.
6. Provide a unit test in `.\src\Promitor.Scraper.Tests.Unit\Serialization\v1\Providers`
  that tests the deserialization based on our sample. Your test class must inherit
  from `ResourceDeserializerTest` to ensure the inherited functionality is tested.

Going forward in this guide, `TResourceDefinition` will refer to your newly created
configuration type.

## Validation

For every scraper type we provide validation for the configuration so that Promitor
fails to start up.

This requires the following steps:

1. Create a new validator that implements `IMetricValidator`. This validator should
  reside in `.\src\Promitor.Scraper.Host\Validation\MetricDefinitions\ResourceTypes`.
  You can look at the contents of `ServiceBusQueueMetricValidator` for an idea of
  the validation inputs, steps, and outputs typical of validator implementation.
2. Add construction and usage of this validator to `.\src\Promitor.Scraper.Host\Validation\Factories\MetricValidatorFactory.cs`
  for the ResourceType you created in step #1 above.
3. Provide a unit test for every validation rule that was added in `.\src\Promitor.Scraper.Tests.Unit\Validation\Metrics\ResourceTypes`

## Scraping

1. Implement a scraper, that inherits from `AzureMonitorScraper<TResourceDefinition>`, which will specify what resource to scrape with Azure Monitor.
2. Hook your new scraper in our `MetricScraperFactory` which determines what scraper
  to use for the passed configuration.

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

This should be provided in a new file under `docs\configuration\metrics`.
