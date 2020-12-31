---
layout: default
title: Migrate to our new Helm chart registry
---

As announced in [#1439](https://github.com/tomkerkhove/promitor/issues/1439), we are replacing our old Helm chart registry
 (`https://promitor.azurecr.io/helm/`) with our new one which is served on `https://charts.promitor.io/` and fully
  available on [GitHub](https://github.com/promitor/charts).

**Our old registry will remain available until April 1st, 2021 when it will be removed.**

## Migrate to new Helm chart registry

Migrating to the new Helm chart registry is easy:

- Remove your current Promitor Helm repo

```cli
❯ helm repo remove promitor
"promitor" has been removed from your repositories
```

- Add our new Promitor Helm repo

```cli
❯ helm repo add promitor https://charts.promitor.io/
"promitor" has been added to your repositories
```

- Update your Helm repos

```cli
❯ helm repo update
Hang tight while we grab the latest from your chart repositories...
...Successfully got an update from the "promitor" chart repository
Update Complete. ⎈ Happy Helming!⎈
```

[&larr; back](/)
