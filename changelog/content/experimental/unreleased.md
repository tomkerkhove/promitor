---
date: 2022-11-22T12:46:47+02:00
weight: 1
version:
---

#### Scraper

- {{% tag added %}} Provide custom formatting for emitting metrics using StatsD sink in Geneva format
- {{% tag added %}} Provide capability to read Azure AD service principal secret from a file
- {{% tag added %}} Provide support for scraping multiple metrics dimensions ([#1820](https://github.com/tomkerkhove/promitor/issues/1820))
- {{% tag added %}} Provide Azure Log Analytics scraper ([docs](https://docs.promitor.io/unreleased/scraping/providers/log-analytics/) | [#2132](https://github.com/tomkerkhove/promitor/pull/2132))
- {{% tag added %}} Provide support for Azure Data Explorer Clusters ([docs](https://docs.promitor.io/unreleased/scraping/providers/data-explorer-clusters.md))
- {{% tag added %}} Provide support for Azure NAT Gateway ([docs](https://docs.promitor.io/unreleased/scraping/providers/nat-gateway.md))
- {{% tag added %}} Provide container vulnerability scanning in CI
- {{% tag added %}} Provide option to use a User Assigned Managed Identity without specifying the Client ID
- {{% tag added %}} Provide support for Public IP Address ([docs](https://docs.promitor.io/unreleased/scraping/providers/public-ip-address.md))
- {{% tag security %}} Patch for [CVE-2023-0286](https://github.com/advisories/GHSA-x4qr-2fvf-3mr5) (Critical | Base image)
- {{% tag security %}} Patch for [CVE-2023-0215](https://github.com/advisories/GHSA-x4qr-2fvf-3mr5) (Critical | Base image)
- {{% tag security %}} Patch for [CVE-2022-41032](https://github.com/advisories/GHSA-g3q9-xf95-8hp5) (High)
- {{% tag security %}} Patch for [CVE-2022-4450](https://github.com/advisories/GHSA-v5w6-wcm8-jm4q) (High | Base image)
- {{% tag security %}} Patch for [CVE-2023-0215](https://github.com/advisories/GHSA-r7jw-wp68-3xch) (High | Base image)
- {{% tag security %}} Patch for [CVE-2022-42898](https://access.redhat.com/security/cve/cve-2022-42898) (High | Base image)
- {{% tag security %}} Patch for [CVE-2022-4304](https://github.com/advisories/GHSA-p52g-cm5j-mjv4) (Moderate | Base image)
- {{% tag fixed %}} Fixed a bug where startup throws scheduling exception due to metric misconfiguration
- {{% tag fixed %}} Fixed a bug where resource discovery for Azure Container Instances was not working
- {{% tag fixed %}} Fixed a bug where Promitor was reported as `unknown_service:dotnet` job in OpenTelemetry Collector
- {{% tag fixed %}} Fixed a bug where OpenTelemetry sink had concurrency issues
- {{% tag changed %}} Migrate to .NET 7
- {{% tag changed %}} Migrate Resharper code quality check to GitHub Actions
- {{% tag deprecated %}} Support for scraping single metric dimension by using `metrics[x].azureMetricConfiguration.dimension` ([#1820](https://github.com/tomkerkhove/promitor/issues/1820))

#### Resource Discovery

- {{% tag added %}} Provide path to read app secret key from file
- {{% tag added %}} Provide container vulnerability scanning in CI
- {{% tag added %}} Provide support for Azure Data Explorer Clusters ([docs](https://docs.promitor.io/unreleased/scraping/providers/data-explorer-clusters.md))
- {{% tag added %}} Provide support for Azure NAT Gateway ([docs](https://docs.promitor.io/unreleased/scraping/providers/nat-gateway.md))
- {{% tag added %}} Provide option to use a User Assigned Managed Identity without specifying the Client ID
- {{% tag added %}} Provide support for Public IP Address ([docs](https://docs.promitor.io/unreleased/scraping/providers/public-ip-address.md))
- {{% tag security %}} Patch for [CVE-2022-37434](https://github.com/advisories/GHSA-cfmr-vrgj-vqwv) (Critical | Base image)
- {{% tag security %}} Patch for [CVE-2021-42377](https://github.com/advisories/GHSA-phvg-gc27-gjwp) (Critical | Base image)
- {{% tag security %}} Patch for [CVE-2022-38013](https://github.com/advisories/GHSA-r8m2-4x37-6592) (High)
- {{% tag security %}} Patch for [CVE-2022-41032](https://github.com/advisories/GHSA-g3q9-xf95-8hp5) (High)
- {{% tag security %}} Patch for [CVE-2023-0215](https://github.com/advisories/GHSA-r7jw-wp68-3xch) (High | Base image)
- {{% tag security %}} Patch for [CVE-2022-2097](https://github.com/advisories/GHSA-3wx7-46ch-7rq2) (High | Base image)
- {{% tag security %}} Patch for [CVE-2021-42373](https://github.com/advisories/GHSA-6w3h-h7gw-72qf) (High | Base image)
- {{% tag security %}} Patch for [CVE-2022-34716](https://github.com/advisories/GHSA-2m65-m22p-9wjw) (Moderate)
- {{% tag security %}} Patch for [CVE-2022-4304](https://github.com/advisories/GHSA-p52g-cm5j-mjv4) (Moderate | Base image)
- {{% tag changed %}} Migrate to .NET 7
- {{% tag changed %}} Migrate Resharper code quality check to GitHub Actions
