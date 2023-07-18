---
date: 2022-11-22T12:46:47+02:00
weight: 1
version:
---

#### Scraper

- {{% tag fixed %}} Improve handling of time series with missing dimensions that are requested ([#2331](https://github.com/tomkerkhove/promitor/issues/2331))
- {{% tag added %}} Provide support for all label scenarios in StatsD & OpenTelemetry metric sink. This includes
dimensions, customer & default labels.
- {{% tag changed %}} Switch to Mariner distroless base images
- {{% tag security %}} Patch for [CVE-2023-29331](https://github.com/advisories/GHSA-555c-2p6r-68mm) (High)

#### Resource Discovery

- {{% tag changed %}} Switch to Mariner distroless base images
- {{% tag security %}} Patch for [CVE-2023-29331](https://github.com/advisories/GHSA-555c-2p6r-68mm) (High)
