---
layout: default
title: Promitor - Bringing Azure Monitor metrics where you need them
---
<img src="https://static.scarf.sh/a.png?x-pxid=3d288d64-88d8-4196-b377-f55dc7ce7269" />
[![License](https://img.shields.io/github/license/mashape/apistatus.svg?style=flat-square)](https://github.com/tomkerkhove/promitor/blob/master/LICENSE)
[![Build Status](https://img.shields.io/azure-devops/build/tomkerkhove/promitor/50/master.svg?label=Scraper%20Agent%20-%20CI&style=flat-square)](https://dev.azure.com/tomkerkhove/Promitor/_build/latest?definitionId=50&branchName=master)
[![Docker Pulls](https://img.shields.io/docker/pulls/tomkerkhove/promitor-agent-scraper.svg?style=flat-square)](https://hub.docker.com/r/tomkerkhove/promitor-agent-scraper/)
[![Docker Stars](https://img.shields.io/docker/stars/tomkerkhove/promitor-agent-scraper.svg?style=flat-square)](https://hub.docker.com/r/tomkerkhove/promitor-agent-scraper/)
[![Artifact Hub](https://img.shields.io/endpoint?url=https://artifacthub.io/badge/repository/promitor?style=flat-square)](https://artifacthub.io/packages/search?repo=promitor)
[![Donate](https://img.shields.io/badge/Donate%20via-GitHub-blue.svg?style=flat-square)](https://github.com/users/tomkerkhove/sponsorship)

**Promitor** is an Azure Monitor scraper which makes the metrics available
for metric systems such as Atlassian Statuspage, Prometheus and StatsD.

{:refdef: style="text-align: center;"}
![Promitor](./media/logos/promitor.png)
{: refdef}

## Running Promitor Scraper

Running Promitor Scraper is super easy:

```shell
docker run -d -p 8999:80 --name promitor-agent-scraper \
                         --env PROMITOR_AUTH_APPID='<azure-ad-app-id>'   \
                         --env-file C:/Promitor/az-mon-auth.creds \
                         --volume C:/Promitor/metrics-declaration.yaml:/config/metrics-declaration.yaml \
                         --volume C:/Promitor/runtime.yaml:/config/runtime.yaml \
                         ghcr.io/tomkerkhove/promitor-agent-scraper:2.0.0-rc
```

Docker image is available on [Docker Hub](https://hub.docker.com/r/tomkerkhove/promitor-agent-scraper/).

## Features

- Automatically scrapes Azure Monitor metrics (single and multi-dimensional) across various subscription &
 resource groups
- Automatically pushes metrics to systems such as Atlassian Statuspage, Prometheus and StatsD
- Easy to declare metrics to scrape via metrics-as-code or automatically discover resources
- Built-in support for a variety of Azure services ([overview](configuration/v2.x/metrics#supported-azure-services))
- Easily deployable via Docker & Kubernetes
- Sends telemetry to container logs & Azure Application Insights
- Available for Linux & Windows runtimes
- Support for all Azure clouds

And there is more on the way - Check our [backlog](https://github.com/tomkerkhove/promitor/issues)
and vote for features!

## Documentation

- **[How It Works](concepts/how-it-works)**
  - [Resource Discovery](concepts/how-it-works#using-resource-discovery)
- **Metrics**
  - [General Declaration](configuration/v2.x/metrics)
  - [Supported Providers](configuration/v2.x/metrics#supported-azure-services)
  - [What labels do we provide?](metrics/labels)
- **Deployment**
  - [Overview](deployment)
  - Scraper
    - [Docker](deployment/scraper/#docker)
    - [Kubernetes](deployment/scraper/#kubernetes)
  - Resource Discovery
    - [Docker](deployment/resource-discovery/#docker)
    - [Kubernetes](deployment/resource-discovery/#kubernetes)
  - [Image Tagging Strategy](deployment#image-tagging-strategy)
  - [Migrate to our new Helm chart registry](/deployment/migrate-to-new-helm-registry)
- **Configuration**
  - Scraper
    - [Overview of metric sinks](configuration/v2.x/runtime/scraper#metric-sinks)
      - [Atlassian Statuspage](configuration/v2.x/runtime/scraper#atlassian-statuspage)
      - [Prometheus Scraping Endpoint](configuration/v2.x/runtime/scraper#prometheus-scraping-endpoint)
      - [StatsD](configuration/v2.x/runtime/scraper#statsd)
    - [Using resource discovery](configuration/v2.x/runtime/scraper#using-resource-discovery)
    - [Authentication with Azure Monitor](configuration/v2.x/azure-authentication)
    - [Logging & External Providers](configuration/v2.x/runtime/scraper#telemetry)
    - [Runtime](configuration/v2.x/runtime/scraper)
  - Resource Discovery
    - [Configuring Runtime](configuration/v2.x/runtime/resource-discovery)
    - [Authentication with Azure Resource Graph](configuration/v2.x/azure-authentication)
    - [Declaring resource discovery groups](configuration/v2.x/resource-discovery)
- **Security**
  - [Authentication with Microsoft Azure](configuration/v2.x/azure-authentication)
    - [Overview of Azure integrations](configuration/v2.x/azure-authentication#overview)
    - [Supported Authentication Mechanisms](configuration/v2.x/azure-authentication#supported-authentication-mechanisms)
    - [Required permissions for Azure Monitor](configuration/v2.x/azure-authentication#required-permissions-for-azure-monitor)
    - [Required permissions for Azure Resource Graph](configuration/v2.x/azure-authentication#required-permissions-for-azure-resource-graph)
- **Operations**
  - [Health](operations#health)
  - [Discovery](operations#discovery)
  - [Performance](operations#performance)
  - [System](operations#system)
  - [Integrations](operations#integrations)
    - [Azure Resource Manager API - Consumption & Throttling](operations#azure-resource-manager-api---consumption--throttling)
    - [Azure Monitor](operations#azure-monitor)
  - [Configuration REST APIs](operations#configuration-rest-apis)
- **Walkthroughs**
  - [Deploying Promitor, Prometheus, and Grafana on an AKS Cluster](/walkthrough/scrape-promitor-with-prometheus-on-azure-kubernetes-service)
  - [Using Managed Identity with Promitor on an AKS Cluster](/walkthrough/use-promitor-with-managed-identity)
  - [Migrate from Promitor Scraper 1.x to 2.x](/walkthrough/migrate-from-1.x-to-2.x)
- [**Frequently asked questions (FAQs)**](/faq)

## Support

Promitor is actively maintained and developed with best-effort support.

We do welcome PRs that implement features from our backlog and are always happy
to help you incorporate Promitor in your infrastructure, but do not provide 24/7
support. Are you having issues or feature requests?

Feel free to [let us know](https://github.com/tomkerkhove/promitor/issues/new/choose)!

## End-users

We are proud to have the following end-users(s) running Promitor in production:

![Adobe](./media/logos/end-users/adobe.png)
![Albert Heijn](./media/logos/end-users/albert-heijn.png)
![Bryte Blue](./media/logos/end-users/bryte-blue.png)
![ResDiary](./media/logos/end-users/resdiary.png)
![The Trade Desk](./media/logos/end-users/the-trade-desk.png)
![Trynz](./media/logos/end-users/trynz.png)
![Vsoft](./media/logos/end-users/vsoft.png)
![Walmart Labs](./media/logos/end-users/walmart-labs.jpg)

Are you a Promitor user? Let us know and [get listed](https://forms.gle/hjcpaaVFa1A1hZaK6)!

Learn more about how they are using Promitor:

- ["Monitor Azure Resources using Promitor"](https://medium.com/resdiary-product-team/monitor-azure-resources-using-promitor-b3d8384867c1)
 by ResDiary

## Thank you

We'd like to thank the following service(s) for supporting our open-source initiative!

- **Netlify** allows us to provide previews of our documentation changes in our
  pull requests that make it easier to review them.

<!-- markdownlint-disable MD033 -->
  <a href="https://www.netlify.com">
    <img src="https://www.netlify.com/img/global/badges/netlify-color-bg.svg" alt="Deploys by Netlify" />
  </a>
<!-- markdownlint-enable -->

But they are not the only one we'd like to thank!

For a full list of services, tooling & NuGet packages that support us -
 Have a look at our [Thank you](thank-you) page!

## License Information

This is licensed under The MIT License (MIT). Which means that you can use, copy,
modify, merge, publish, distribute, sublicense, and/or sell copies of the web application.
But you always need to state that Tom Kerkhove is the original author of this web
application.
