---
date: 2021-01-01T20:46:47+02:00
weight: 1
version:
---

#### Scraper

- {{% tag added %}} Provide scraper for Azure Database for MySQL Servers  ([docs](https://docs.promitor.io/v2.x/scraping/providers/mysql/)
 | [#1880](https://github.com/tomkerkhove/promitor/issues/324))
- {{% tag fixed %}} Honor flag not to include timestamps in system metrics for Prometheus ([#1915](https://github.com/tomkerkhove/promitor/pull/1915))
- {{% tag fixed %}} Improve performance for scraping large amoung of Azure resrouces to reduce CPU usage ([#1834](https://github.com/tomkerkhove/promitor/issues/1834))

#### Resource Discovery

- {{% tag added %}} Provide scraper for Azure Database for MySQL Servers ([docs](https://docs.promitor.io/v2.x/scraping/providers/mysql/)
 | [#1880](https://github.com/tomkerkhove/promitor/issues/324))
- {{% tag fixed %}} Ensure Resource Discovery background jobs handle paging well  to reduce CPU usage ([#2018](https://github.com/tomkerkhove/promitor/issues/2018))
