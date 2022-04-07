# Docker

```shell
❯ docker run -d -p 9999:80 --name promitor-agent-resource-discovery   \
                         --env PROMITOR_AUTH_APPID='<azure-ad-app-id>'   \
                         --env-file C:/Promitor/promitor-discovery-auth.creds   \
                         --volume C:/Promitor/resource-discovery-declaration.yaml:/config/resource-discovery-declaration.yaml   \
                         --volume C:/Promitor/resource-discovery-runtime.yaml:/config/runtime.yaml   \
                         ghcr.io/tomkerkhove/promitor-agent-resource-discovery:0.1.0-preview-1
```
