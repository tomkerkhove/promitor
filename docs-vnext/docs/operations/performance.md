# Performance

![Resource Discovery Support Badge](https://img.shields.io/badge/Support%20for%20Resource%20Discovery-Yes-green.svg)
![Scraper Support Badge](https://img.shields.io/badge/Support%20for%20Scraper-Yes-green.svg)

You can easily monitor the performance of Promitor through the following Prometheus metrics:

- `promitor_runtime_dotnet_collection_count_total` - Provides information related to garbage collection count per generation
- `promitor_runtime_dotnet_totalmemory` - Provides information related to total known allocated memory
- `promitor_runtime_process_cpu_seconds_total` - Provides information related to total user & system CPU time spent in seconds
- `promitor_runtime_process_virtual_bytes` - Provides information related to virtual memory size
- `promitor_runtime_process_working_set` - Provides information related to process working set
- `promitor_runtime_process_private_bytes` - Provides information related to process private memory size
- `promitor_runtime_process_num_threads` - Provides information related to total number of threads
- `promitor_runtime_process_processid` - Provides information related to process ID
- `promitor_runtime_process_start_time_seconds` - Provides information related to the start time of the process since
 unix epoch in seconds
- `promitor_runtime_http_request_duration_seconds` - Provides information related to the performance of HTTP routes and outcomes

```text
# HELP promitor_runtime_http_request_duration_seconds duration histogram of http responses labeled with: status_code, method, path
# TYPE promitor_runtime_http_request_duration_seconds histogram
promitor_runtime_http_request_duration_seconds_bucket{status_code="200",method="GET",path="/scrape",le="0.005"} 30
promitor_runtime_http_request_duration_seconds_bucket{status_code="200",method="GET",path="/scrape",le="0.01"} 31
promitor_runtime_http_request_duration_seconds_bucket{status_code="200",method="GET",path="/scrape",le="0.025"} 31
promitor_runtime_http_request_duration_seconds_bucket{status_code="200",method="GET",path="/scrape",le="0.05"} 32
promitor_runtime_http_request_duration_seconds_bucket{status_code="200",method="GET",path="/scrape",le="0.075"} 33
promitor_runtime_http_request_duration_seconds_bucket{status_code="200",method="GET",path="/scrape",le="0.1"} 33
promitor_runtime_http_request_duration_seconds_bucket{status_code="200",method="GET",path="/scrape",le="0.25"} 34
promitor_runtime_http_request_duration_seconds_bucket{status_code="200",method="GET",path="/scrape",le="0.5"} 34
promitor_runtime_http_request_duration_seconds_bucket{status_code="200",method="GET",path="/scrape",le="0.75"} 34
promitor_runtime_http_request_duration_seconds_bucket{status_code="200",method="GET",path="/scrape",le="1"} 34
promitor_runtime_http_request_duration_seconds_bucket{status_code="200",method="GET",path="/scrape",le="2.5"} 34
promitor_runtime_http_request_duration_seconds_bucket{status_code="200",method="GET",path="/scrape",le="5"} 34
promitor_runtime_http_request_duration_seconds_bucket{status_code="200",method="GET",path="/scrape",le="7.5"} 34
promitor_runtime_http_request_duration_seconds_bucket{status_code="200",method="GET",path="/scrape",le="10"} 34
promitor_runtime_http_request_duration_seconds_bucket{status_code="200",method="GET",path="/scrape",le="+Inf"} 34
promitor_runtime_http_request_duration_seconds_sum{status_code="200",method="GET",path="/scrape"} 0.27116070000000003
promitor_runtime_http_request_duration_seconds_count{status_code="200",method="GET",path="/scrape"} 34
```

## Scraping Prometheus endpoint

Every Promitor agent supports exposing Prometheus metrics:

- **Resource Discovery agent** - Exposed on `/metrics` endpoint
- **Scraper agent** - Exposed through Prometheus metric sink ([docs](/configuration/v2.x/runtime/scraper#prometheus-scraping-endpoint))

## Scraping Azure Monitor

You can easily monitor the performance of Promitor Scraper agent integrating with Azure Monitor
 through the following Prometheus metrics:

- `promitor_scrape_error` - Provides indication of all configured metrics that were unable to be scraped in Azure Monitor

```prom
# HELP promitor_scrape_error Provides an indication that the scraping of the resource has failed
# TYPE promitor_scrape_error gauge
promitor_scrape_error{metric_name="promitor_demo_app_insights_dependency_duration_200_OK",resource_group="docker-hub-metrics",resource_name="Microsoft.Insights/Components/docker-hub-metrics",resource_type="Generic",subscription_id="0f9d7fea-99e8-4768-8672-06a28514f77e",tenant_id="e0372f7f-a362-47fb-9631-74a5c4ba8bbf"} 1 1623691623231
```

- `promitor_scrape_success` - Provides indication of all configured metrics that were successfully scraped and reported in
the configured metric sinks

```text
# HELP promitor_scrape_success Provides an indication that the scraping of the resource was successful
# TYPE promitor_scrape_success gauge
promitor_scrape_success{metric_name="promitor_demo_automation_update_deployment_machine_runs",resource_group="promitor-sources",resource_name="promitor-sandbox",resource_type="AutomationAccount",subscription_id="0f9d7fea-99e8-4768-8672-06a28514f77e",tenant_id="e0372f7f-a362-47fb-9631-74a5c4ba8bbf"} 1 1623691626335
```
