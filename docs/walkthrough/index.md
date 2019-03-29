---
layout: default
title: Walkthroughs & Tutorials
---

All current walkthroughs & tutorials can be found here.

# Helm

```yaml
azureMetadata:
  tenantId: <azure-ad-app-id>
  subscriptionId: <azure-ad-app-key>
  resourceGroupName: <resource-group-name>
metricDefaults:
  aggregation:
    interval: 00:05:00
  scraping:
    schedule: "* * * * *"
metrics:
  - name: demo_queue_size
    description: "Amount of active messages of the <queue-name> queue"
    resourceType: ServiceBusQueue
    namespace: <service-bus-namespace>
    queueName: <queue-name>
    azureMetricConfiguration:
      metricName: ActiveMessages
      aggregation:
        type: Total
        interval: 00:01:00
```

```bash
helm install ./charts/promitor-scraper \
             --set azureAuthentication.appId='<azure-ad-app-id>' \
             --set azureAuthentication.appKey='<azure-ad-app-key>' \
             --values /path/to/metric-declaration.yaml
```

should give you an output that includes a script similar to this one:

```bash
cat > promitor-scrape-config.yaml <<EOF
extraScrapeConfigs: |
  - job_name: punk-rattlesnake-promitor-scraper
    metrics_path: /prometheus/scrape
    static_configs:
      - targets:
        - punk-rattlesnake-promitor-scraper.default.svc.cluster.local:80
EOF
helm install stable/prometheus -f promitor-scrape-config.yaml
```

(You can see this output again at any time by running `helm status <release-name>`.)

Running those will create a Prometheus scraping configuration file & deploy Prometheus to your cluster with that scraping configuration in addition to the default. From there, run `kubectl port-forward svc/<prometheus-release-name>-prometheus-server 9090:80`. This will allow you to view the Prometheus server at http://localhost:9090. There, you should be able to query $demo_queue_size and see a result (once all pods are up & Promitor has scraped metrics at least once - run `kubectl get pods` to see the status of your pods). 

[&larr; back](/)