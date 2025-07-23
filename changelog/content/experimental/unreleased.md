---
date: 2022-11-22T12:46:47+02:00
weight: 1
version:
---

#### Scraper

- {{% tag fixed %}} Azure Monitor Scraper: Fixed a breaking issue where duplicate metric namespaces returned by the Azure API would cause Promitor to fail in some regions (notably Germany West Central). The scraper now deduplicates namespaces and logs a warning if multiple are found, using the first available namespace. This makes Promitor resilient to recent Azure API changes.

#### Resource Discovery

None.
