# Adding a new scraper type
This guide walks you through the process of adding a new scraper type.

------------------------

:loudspeaker: _Before implementing a new scraping type, please open an issue to discuss your scenario_

-------------------------

## Configuration
1. Add your new scraping type to the  `Promitor.Core.Scraping.Configuration.Model.ResourceType`
2. Describe the resource for which you're creating scraping metrics by creating `<New-Type>MetricDefinition`and inherit from `Promitor.Core.Scraping.Configuration.Model.Metrics.MetricDefinition` - this class should go in `.\src\Promitor.Core.Scraping\Configuration\Model\Metrics\ResourceTypes`
3. Create a new Deserializer in `.\src\Promitor.Core.Scraping\Configuration\Serialization\Deserializers`. This must inherit from `AzureMetricDeserializer`.
3. Update `Promitor.Core.Scraping.Factories.MetricDeserializerFactory` to handle your new resource type by returning a new instance of the Deserializer you created in the previous step.
4. Provide a unit test in `.\src\Promitor.Scraper.Tests.Unit\Serialization\MetricsDeclaration\` that tests the deserialization based on our sample

Going forward in this guide, `TMetricDefinition` will refer to your newly created configuration type.

## Validation
For every scraper type we provide validation for the configuration so that Promitor fails to start up.

This requires the following steps:
1. Create a new validator that implements `IMetricValidator<TMetricDefinition>`
2. Hook your new validator in our validation pipeline in `MetricsValidator`
3. Provide a unit test for every validation rule that was added

## Scraping
1. Implement a scraper that inherits from `Scraper<TMetricDefinition>`. This one will specify how to call Azure Monitor.
2. Hook your new scraper in our `MetricScraperFactory` which determines what scraper to use for the passed configuration

---------------------------

:memo: _Currently we don't have integration tests_

---------------------------

## Documentation
Features are great to have but without clear & well-written documentation they are somewhat useless.

It would be good if you could provide documentation on the following:
1. What Azure service it supports and how to use it
2. What fields need to be configured and what they are for
3. An example configuration

This should be provided in a new file under `docs\configuration\metrics`.
