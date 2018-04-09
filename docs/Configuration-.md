---
layout: default
title: Promitor - Configuration
---

Coming soon.

## Environment variables

- **PROMITOR_CONFIGURATION_PATH** *(Mandatory)* - Location of the YAML file describing what metrics to query in Azure Monitor
- **PROMITOR_SCRAPEENDPOINT_BASEPATH** *(Optional)* - Path where the scrape endpoint for Prometheus will be exposed.
   - Default is `/prometheus/scrape`
- **PROMITOR_SCRAPE_SCHEDULE** *(Optional)* - Cron schedule to scrape Azure Monitor for all metrics.
   - Default is `*/5 * * * *`

[&larr; back](.)