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
We currently provide [a sample declaration](https://github.com/tomkerkhove/promitor/tree/master/deploy) which deploys all the required infrastructure on your Kubernetes cluster.

Once downloaded, you can deploy it by running this command:
```
❯ kubectl apply --file .\deploy\kubernetes-spec.yaml --namespace promitor
```

Want to use Helm? Make sure to vote for [this feature](https://github.com/tomkerkhove/promitor/issues/17).

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
