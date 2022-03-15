## What labels do we provide?

Here is an overview of how we label metrics.

There are a couple of scenarios where labels are being added:

- Built-in labels
- Metric dimension labels
- Scaler-specific labels
- Bring-your-own labels

In case of duplicate label names the above priority will be used.

## Built-in labels

Every metric that is being reported in the scraping endpoint comes with the following
built-in labels:

- `resource_uri` - Full resource URI of the instance. *(ie `subscriptions/xxx/resourceGroups/yyy/providers/Microsoft.ServiceBus/namespaces/promitor`)*
- `subscription_id` - Id of the subscription.
- `resource_group` - Name of the resource group.
- `instance_name` - Name of the instance, if applicable.

## Metric dimension labels

Metrics support specifying a dimension which will be scraped in Azure Monitor.

Every metric value will be reported under the configured metric name, but a
label for the dimension and it's respective value will be added.

## Scraper-specific labels

Every scraper can provide additional labels to provide more information.

Currently we support this for:

- Azure API Management
- Azure Function App
- Azure Service Bus
- Azure SQL Database
- Azure Storage Queue
- Azure Web App

For more information, we recommend reading the [scraper-specific documentation](./../configuration/v1.x/metrics/#supported-azure-services).

## Custom Labels

As of v1.0, you can define your own labels by configuring them for a specific metric.

For more information, see ["Metrics"](./../configuration/v1.x/metrics/#metrics).
