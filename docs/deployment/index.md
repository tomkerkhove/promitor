---
layout: default
title: Deploying Promitor
---

Here is an overview of how you can deploy Promitor. 

_For more information about advanced configuration, read our documentation [here](/configuration)._

# Docker

```
❯ docker run -d -p 8999:80 -e PROMITOR_AUTH_APPID='<azure-ad-app-id>'   \
                         -e PROMITOR_AUTH_APPKEY='<azure-ad-app-key>' \
                         -v C:/Promitor/metrics-declaration.yaml:/config/metrics-declaration.yaml \ 
                         tomkerkhove/promitor-scraper
```

# Kubernetes
We currently provide [a helm chart](https://github.com/tomkerkhove/promitor/tree/master/charts) which deploys all the required infrastructure on your Kubernetes cluster.

Some basic commands to get you up and running:
- az group create -n promitor -l westus2
- az aks create -n promitor -g promitor -l westus2 --generate-ssh-keys
- az ad sp create-for-rbac --role='Contributor' --scope="/subscriptions/<subscriptionId>/resourceGroups/promitor"

Which will return back an Application Id, Application Password, and Tenant Id.

You can then edit the values.yaml, or take a look at charts/local-values.yaml.example to create your own and see which values are needed to minimally run the chart.

After filling out the required fields, you can then deploy the chart by running this command:
```
❯ helm install --name promitor .\charts\promitor --namespace promitor --values \charts\promitor\values.yaml
```


# Image Tagging Strategy
Depending on your scenario you might need a different update cadence for Docker dependencies.

We provide a few options by offering multiple Docker tags:

- **latest** - Ideal for experimentation and proof-of-concepts, but not recommended for running production workloads.
- **{major}.{minor}** - Representation of a specific feature set, but will be updated with feature & security patches.
- **{major}.{minor}.{patch}** - Run a specific version of the runtime.
_(Alternative could be to use [image digest pinning](https://docs.docker.com/engine/reference/commandline/pull/#pull-an-image-by-digest-immutable-identifier))_

![Image Tagging Strategy](./../media/deploy-image-tagging-strategy.png)

You can also pin to a specific digest of an image to ensure that you are running the same image across your infrastructure.
However, you will not receive security patches unless you use a tool like [Renovate to keep them up-to-date](https://renovatebot.com/blog/docker-mutable-tags).

[&larr; back](/)
