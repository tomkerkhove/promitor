---
title:
subtitle: created on {{ now.Format "2020-11-23" }}
date: 2020-11-23T12:00:00+01:00
removal_date: 2020-11-23
weight:
version:
---

#### Helm 2.0

###### Deprecated as of v1.6 and will be removed in v2.0

Promitor provides Helm charts to simplify deployments on a Kubernetes cluster.

In November 2019, Helm 3 was announced which marked the end of Helm 2 which would be phased out in a year and no longer
 supported. Learn more about it in the official [Helm deprecation timeline](https://helm.sh/blog/helm-v2-deprecation-timeline/).

Because of this deprecation, Promitor will no longer support Helm 2.0 deployments out-of-the-box. They might still be
 working but won't be our focus for versions as of Promitor 2.0.

**Announcement:** [GitHub Issue](https://github.com/tomkerkhove/promitor/issues/1371)

**Impact:** Migration is required - Helm 2.0 deployments might still be working but is no longer supported.

**Alternative:** Use Helm 3.0 by following [Helm migration guide](https://helm.sh/docs/topics/v2_v3_migration/).

**Discussion:** [GitHub Discussions](https://github.com/tomkerkhove/promitor/discussions/1375)
