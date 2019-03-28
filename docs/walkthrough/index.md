---
layout: default
title: Walkthroughs & Tutorials
---

All current walkthroughs & tutorials can be found here.

# Helm

```
helm install ./charts/promitor-scraper \
             --set azureAuthentication.appId='<azure-ad-app-id>' \
             --set azureAuthentication.appKey='<azure-ad-app-key>' \
             --values /path/to/metric-declaration.yaml
```

should give you an output that includes

```
3. To set up Prometheus in your cluster & pull in metrics from Promitor's scraping output, run:

export CLUSTER_IP=$(kubectl get svc --namespace default -l "app=promitor-scraper,release=punk-rattlesnake" -o jsonpath="{.items[0].spec.clusterIP}")
cat > promitor-scrape-config.yaml <<EOF
extraScrapeConfigs: |
  - job_name: punk-rattlesnake
    metrics_path: /prometheus/scrape
    static_configs:
      - targets:
        - $CLUSTER_IP:80
EOF
helm install stable/prometheus -f promitor-scrape-config.yaml
```

(You can see this output again aat any time by running `helm status <release-name>`.)

Running those commands should give you output that includes

```
Get the Prometheus server URL by running these commands in the same shell:
  export POD_NAME=$(kubectl get pods --namespace default -l "app=prometheus,component=server" -o jsonpath="{.items[0].metadata.name}")
  kubectl --namespace default port-forward $POD_NAME 9090
```

Running these will allow you to view the Prometheus server at http://localhost:9090. There, you should be able to query $metric-name and see a result (once all pods are up & Promitor has scraped metrics at least once).



[&larr; back](/)