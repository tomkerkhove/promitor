---
layout: default
title: Promitor - An Azure Monitor scraper for Prometheus
---

Promitor is an Azure Monitor scraper for Prometheus. It provides a scraping endpoint for Prometheus that provides a subset of Azure Monitor metrics inside your cluster.

----------------------------

:rotating_light: This is under active development and not ready yet. Expect some (major) changes.

----------------------------

# Features

- Automatically scrape Azure Monitor metrics
- Provides scraping endpoint for Prometheus
- Easily deployable via a Docker image

## Supported Built-In Metrics

- Azure Service Bus Queue/Topic

# Running Promitor Scraper
Running Promitor Scraper with the default metrics configuration is super easy:
```
docker run -d -p 8999:80 tomkerkhove/promitor-scraper:alpha
```

# Documentation
All documentation can be found in the [wiki](https://github.com/tomkerkhove/promitor/wiki).

Here are some interesting topics:
- [Configuration (wiki)](https://github.com/tomkerkhove/promitor/wiki/Configuration)
- [Configuration (Page)](configuration)
- [Configuration (Page - MD)](configuration.md)

# License Information
This is licensed under The MIT License (MIT). Which means that you can use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the web application. But you always need to state that Codit is the original author of this web application.
