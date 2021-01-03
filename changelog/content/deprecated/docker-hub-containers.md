---
title:
subtitle: created on {{ now.Format "2020-12-31" }}
date: 2021-01-03T01:00:00+01:00
removal_date:
weight:
version:
---

#### Moving container images from Docker Hub to GitHub Container Registry

###### Deprecated as of Janurary 1st, 2021 and newer versions will be published to GitHub Container Registry only

Promitor provides Docker images to deploy agents on any container orchestrator supporting either Linux or Windows.
 You can find them on Docker Hub and easily pull them on your nodes.

In 2020, however, Docker Hub announced changes to their image retention & rate limiting
 which can impact Promitor customers which is why we are moving our images to [GitHub Container Registry](https://github.blog/2020-09-01-introducing-github-container-registry/).

This allows us to bring our artifacts closer to the project itself and improve how we manage Promitor.

**Announcement:** [GitHub Issue](https://github.com/tomkerkhove/promitor/issues/1444)

**Impact:** Migration is required for newer versions - Use our new Docker images on [GitHub Container Registry](https://github.com/tomkerkhove?tab=packages&repo_name=promitor&ecosystem=container).

**Alternative:** None

**Discussion:** [GitHub Discussions](https://github.com/tomkerkhove/promitor/discussions/1445   )
