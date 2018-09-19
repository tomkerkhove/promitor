---
layout: default
title: Promitor - An Azure Monitor scraper for Prometheus
---

[![License](https://img.shields.io/github/license/mashape/apistatus.svg?style=flat-square)](https://github.com/tomkerkhove/promitor/blob/master/LICENSE)[![Build Status](https://travis-ci.com/tomkerkhove/promitor.svg?branch=master)](https://travis-ci.com/tomkerkhove/promitor) [![Docker Pulls](https://img.shields.io/docker/pulls/tomkerkhove/promitor-scraper.svg?style=flat-square)](https://hub.docker.com/r/tomkerkhove/promitor-scraper/)
[![Docker Stars](https://img.shields.io/docker/stars/tomkerkhove/promitor-scraper.svg?style=flat-square)](https://hub.docker.com/r/tomkerkhove/promitor-scraper/)

**Promitor** is an **Azure Monitor scraper for Prometheus** providing a scraping endpoint for Prometheus that provides a configured subset of Azure Monitor metrics.

{:refdef: style="text-align: center;"}
![Promitor](./media/logos/promitor.png)
{: refdef}

# Running Promitor Scraper
Running Promitor Scraper is super easy:
```
docker run -d -p 8999:80 --name promitor-scraper
                         --env PROMITOR_AUTH_APPID='<azure-ad-app-id>'   \
                         --env PROMITOR_AUTH_APPKEY='<azure-ad-app-key>' \
                         --volume C:/Promitor/metrics-declaration.yaml:/config/metrics-declaration.yaml \ 
                         tomkerkhove/promitor-scraper:0.2.0
```

Docker image is available on [Docker Hub](https://hub.docker.com/r/tomkerkhove/promitor-scraper/).

# Features

- Automatically scrapes Azure Monitor metrics
- Provides scraping endpoint for Prometheus
- Built-in support for a variety of Azure services ([overview](configuration/metrics))
- Easy to declare metrics to scrape via YAML & APIs
- Easily deployable via Docker & Kubernetes
- Sends telemetry to Azure Application Insights

And there is more on the way - Check our [backlog](https://github.com/tomkerkhove/promitor/issues) and vote for features!

# Known Limitations
- Metrics interval does not take scraping cron schedule into account ([#60](https://github.com/tomkerkhove/promitor/issues/60))

# Documentation
- **Deployment**
    - [Running Promitor on Docker](deployment#docker)
    - [Running Promitor on Kubernetes](deployment#kubernetes)
- **Configuration**
    - [Scraping](configuration#scraping)
    - [Authentication with Azure Monitor](configuration#authentication-with-azure-monitor)
    - [Metrics Declaration](configuration/metrics)
    - [Telemetry](configuration#telemetry)

# Thank you!
We'd like to thank all the services, tooling & NuGet packages that support us - [Thank you](thank-you)!

# License Information
This is licensed under The MIT License (MIT). Which means that you can use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the web application. But you always need to state that Codit is the original author of this web application.
