---
title:
subtitle: created on {{ now.Format "2020-12-31" }}
date: 2020-12-31T01:00:00+01:00
removal_date: 2021-04-1
weight:
version:
---

#### Helm Chart Registry

###### Deprecated as of Janurary 1st, 2021 and will be removed on April 1st, 2021

Promitor provides Helm charts to simplify deployments on a Kubernetes cluster which are hosted on Azure Container Registry.

Unfortunately, Azure Container Registry has dedicated URLs that pinpoints the Azure Container Registry instance. 
This is not ideal given it will break customers if we decide to move our Helm charts elsewhere. Because of this, we are
 introducing a new Helm chart registry which is hosted on [GitHub](https://github.com/promitor/charts) and served on `https://charts.promitor.io/`.

This allows us to reduce the cost and avoid the tight-coupling for customers on Microsoft Azure. We are now future-ready
 in case we need to move our Helm chart registry.

**Announcement:** [GitHub Issue](https://github.com/tomkerkhove/promitor/issues/1371)

**Impact:** Migration is required - Use our new Helm chart registry on `https://charts.promitor.io` or go to [Artifact Hub](https://artifacthub.io/packages/search?page=1&repo=promitor).

**Alternative:** None

**Discussion:** [GitHub Discussions](https://github.com/tomkerkhove/promitor/discussions/1440)
