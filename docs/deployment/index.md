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
                         tomkerkhove/promitor-scraper:0.2.0
```

# Kubernetes
We currently provide [a sample declaration](https://github.com/tomkerkhove/promitor/tree/master/deploy) which deploys all the required infrastructure on your Kubernetes cluster.

Once downloaded, you can deploy it by running this command:
```
❯ kubectl apply --file .\deploy\kubernetes-spec.yaml --namespace promitor
```

Want to use Helm? Make sure to vote for [this feature](https://github.com/tomkerkhove/promitor/issues/17).

[&larr; back](/)
