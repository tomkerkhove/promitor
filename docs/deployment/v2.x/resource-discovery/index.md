---
layout: default
title: Deploying Promitor Resource Discovery
---

Here is an overview of how you can deploy Promitor Resource Discovery on your infrastructure, we support both Linux and Windows.

TODO: Deployment of Resource Discovery

docker run -d -p 9999:80 --name promitor-agent-resource-discovery
                         --env PROMITOR_DISCOVERY_APPID='<azure-ad-app-id>'   \
                         --env-file C:/Promitor/promitor-discovery-auth.creds
                         --volume C:/Promitor/resource-discovery-declaration.yaml:/config/resource-discovery-declaration.yaml
                         --volume C:/Promitor/resource-discovery-runtime.yaml:/config/runtime.yaml
                         tomkerkhove/promitor-agent-discovery:0.1.0-preview-1

[&larr; back](/)
