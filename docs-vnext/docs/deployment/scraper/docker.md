# Docker

```shell
❯ docker run -d -p 8999:80 --name promitor-agent-scraper \
                           --env PROMITOR_AUTH_APPID='<azure-ad-app-id>' \
                           --env-file C:/Promitor/az-mon-auth.creds \
                           --volume C:/Promitor/metrics-declaration.yaml:/config/metrics-declaration.yaml \
                           --volume C:/Promitor/runtime.yaml:/config/runtime.yaml \
                           ghcr.io/tomkerkhove/promitor-agent-scraper:2.0.0-rc
