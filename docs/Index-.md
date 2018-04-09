---
layout: default
title: Promitor - An Azure Monitor scraper for Prometheus
---
[![License](https://img.shields.io/github/license/mashape/apistatus.svg)](./LICENSE)[![Build Status](https://travis-ci.org/tomkerkhove/promitor.svg?branch=master)](https://travis-ci.org/tomkerkhove/promitor)

**Promitor** is an **Azure Monitor scraper for Prometheus** providing a scraping endpoint for Prometheus that provides a subset of Azure Monitor metrics inside your cluster.

----------------------------

:rotating_light: This is under active development and not ready yet. Expect some (major) changes.

----------------------------

# Running Promitor Scraper
Running Promitor Scraper with the default metrics configuration is super easy:
```
docker run -d -p 8999:80 tomkerkhove/promitor-scraper:alpha
```

# Features

- Automatically scrape Azure Monitor metrics
- Provide scraping endpoint for Prometheus
- Easily deployable via a Docker image

## Supported Built-In Metrics

- Azure Service Bus Queue

# Documentation
- [Configuration](Configuration)

# License Information
This is licensed under The MIT License (MIT). Which means that you can use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the web application. But you always need to state that Codit is the original author of this web application.
