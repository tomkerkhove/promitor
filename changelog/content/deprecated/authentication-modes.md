---
title:
subtitle: created on {{ now.Format "2021-04-05" }}
date: 2021-04-05T01:00:00+01:00
#removal_date: 2021-04-1
weight:
version:
---

#### Service Principle identity is moved to runtime configuration

###### Deprecated as of April 5th, 2021 and will be removed in new major version

Promitor has been using service principle authentication from its inception where the identity
inforation was configured through the following environment variables:

- `PROMITOR_AUTH_APPKEY`
- `PROMITOR_AUTH_APPID`

However, as of Promitor Scraper v2.2.0 & Resource Discovery v0.3.0, users can choose how agents authenticate to Microsoft
 Azure by declaring the authentication mode in the server configuration:

```yaml
authentication:
  # Options are ServicePrincipal, SystemAssignedManagedIdentity, UserAssignedManagedIdentity.
  mode: ServicePrincipal
  identityId: xxxx-xxxx-xxxx
```

With this new approach, users can use Managed Identity authentication leveraging no-secret authentication or keep on
 using Service Principle authentication.

**Announcement:** [GitHub Issue](https://github.com/tomkerkhove/promitor/issues/1582)

**Impact:** Migration is required - Use the [authentication configuration](https://docs.promitor.io/configuration/v2.x/azure-authentication#supported-authentication-mechanisms)
 to specify Service Principle authentication and configure the identity id in the server configuration.

**Alternative:** None

**Discussion:** [GitHub Discussions](https://github.com/tomkerkhove/promitor/discussions/1583)
