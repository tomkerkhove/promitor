# Promitor

[Promitor](https://promitor.io/) is an Azure Monitor scraper for Prometheus providing a scraping endpoint for Prometheus that provides a configured subset of Azure Monitor metrics.

## TL;DR

```console
$ helm repo add promitor https://promitor.azurecr.io/helm/v1/repo
$ helm install promitor/promitor-agent-scraper
```

## Introduction

This chart bootstraps a **Promitor Scraper Agent** deployment on a Kubernetes cluster using the Helm package manager.

## Prerequisites

None.

## Installing the Chart

To install the chart with the release name `promitor-agent-scraper`:

```console
$ helm install --name promitor-agent-scraper promitor/promitor-agent-scraper \
               --set azureAuthentication.appId='<azure-ad-app-id>' \
               --set azureAuthentication.appKey='<azure-ad-app-key>' \
               --values /path/to/metric-declaration.yaml
```

The command deploys Prometheus on the Kubernetes cluster with the specified metrics declaration, for more information see [our documentation](https://promitor.io/deployment/#using-our-helm-chart).

## Uninstalling the Chart

To uninstall/delete the `promitor-agent-scraper` deployment:

```console
$ helm delete promitor-agent-scraper
```

The command removes all the Kubernetes components associated with the chart and deletes the release.