---
layout: default
title: Promitor - An Azure Monitor scraper for Prometheus
---
[![License](https://img.shields.io/github/license/mashape/apistatus.svg&style=flat-square)](https://github.com/tomkerkhove/promitor/blob/master/LICENSE)[![Build Status](https://travis-ci.com/tomkerkhove/promitor.svg?branch=master)](https://travis-ci.com/tomkerkhove/promitor) [![Docker Pulls](https://img.shields.io/docker/pulls/tomkerkhove/promitor-scraper.svg&style=flat-square)](https://hub.docker.com/r/tomkerkhove/promitor-scraper/)
[![Docker Stars](https://img.shields.io/docker/stars/tomkerkhove/promitor-scraper.svg&style=flat-square)](https://hub.docker.com/r/tomkerkhove/promitor-scraper/)

**Promitor** is an **Azure Monitor scraper for Prometheus** providing a scraping endpoint for Prometheus that provides a subset of Azure Monitor metrics inside your cluster.

----------------------------

:rotating_light: This is under active development and not ready yet. Expect some (major) changes.

----------------------------

# Running Promitor Scraper
Running Promitor Scraper is super easy:
```
docker run -d -p 8999:80 -e PROMITOR_AUTH_APPID='<azure-ad-app-id>'   \
                         -e PROMITOR_AUTH_APPKEY='<azure-ad-app-key>' \
                         -v C:/Promitor/metrics-declaration.yaml:/config/metrics-declaration.yaml \ 
                         tomkerkhove/promitor-scraper:v0.1
```

Docker image is available on [Docker Hub](https://hub.docker.com/r/tomkerkhove/promitor-scraper/).

# Features

- Automatically scrapes Azure Monitor metrics
- Provides scraping endpoint for Prometheus
- Built-in support for a variety of Azure services ([overview](configuration/metrics))
- Easy to declare metrics to scrape via YAML & APIs
- Easily deployable via Docker & Kubernetes
- More on the way ([backlog](https://github.com/tomkerkhove/promitor/issues))

# Known Limitations
- Metrics interval does not take scraping cron schedule into account ([#60](https://github.com/tomkerkhove/promitor/issues/60))

# Documentation
- **Configuration**
    - [Scraping](configuration#scraping)
    - [Authentication with Azure Monitor](configuration#authentication-with-azure-monitor)
    - [Metrics Declaration](configuration/metrics)

# Acknowledgments

- [Swashbuckle.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) - Swagger tools for documenting API's built on ASP.NET Core
- [Prometheus.Client](https://github.com/PrometheusClientNet/Prometheus.Client) - .NET client for prometheus.io
- [Shuttle.Core.Cron](https://github.com/Shuttle/Shuttle.Core.Cron) - Cron expression parsing
- [YamlDotNet](https://github.com/aaubry/YamlDotNet) - .NET library for YAML

# License Information
This is licensed under The MIT License (MIT). Which means that you can use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the web application. But you always need to state that Codit is the original author of this web application.
