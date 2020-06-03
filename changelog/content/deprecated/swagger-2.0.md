---
subtitle: created on {{ now.Format "2020-01-07" }}
date: 2018-09-02T20:46:47+02:00
removal_date: 2020-01-07
weight:
version:
---

#### Swagger 2.0

###### Deprecated as of v1.1 and will be removed in v2.0

OpenAPI v3.0 has been released in 2017 which is the new industry standard.
With Promitor, we want to support that standard and have decided to go forward
with 3.0 instead of 2.0.

We've added support for OpenAPI 3.0 in Promitor v1.1 next to Swagger 2.0 so you
can migrate to it before we remove 2.0 in Promitor v2.0.

**Impact:** Migration is required - API documentation will no longer be available.

**Alternative:** Use API documentation provided in OpenAPI v3.0 format.

**Discussion:** [GitHub Issue #782](https://github.com/tomkerkhove/promitor/issues/782)
