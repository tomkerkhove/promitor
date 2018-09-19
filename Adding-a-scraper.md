# Adding a new scraper

## Configuration
1. Add a new type
Add your new scraping type to `ResourceType`
Describe your configuration and inherit from `MetricDefinition`
3. Implement a scraper located in //
4. Update the deserialization
`MetricsDeserializer`
2. Test for deserialization

## Validation
For every scraper type we provide validation for the configuration so that Promitor fails to start up.

This requires the following steps:
1. Create a new validator that implements `IMetricValidator<TMetricDefinition>`
2. Hook your new validator in our validation pipeline in (`MetricsValidator`)
3. Provide a unit test for every validation rule that was added

## Samples
Provide a sample configuration in `samples\promitor-sample.yaml`

## Documentation
TBD