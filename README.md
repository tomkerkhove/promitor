# Promitor - Azure Monitor Scraper for Prometheus 
[![License](https://img.shields.io/github/license/mashape/apistatus.svg)](./LICENSE)[![Build Status](https://travis-ci.com/tomkerkhove/promitor.svg?token=GsSXSXe5xF8ZdYK5qExq&branch=master)](https://travis-ci.com/tomkerkhove/promitor)

Azure Monitor Scraper for Prometheus

----------------------------

:rotating_light: This is under active development and subject to change.

----------------------------

# Build & Running the image
1. Move to the project folder
```
❯ cd src\Promitor.Scraper
``` 

2. Build the docker image
```
❯ docker build . -t promitor-scraper
```

3. Run it locally with the default scrape configuration with no metrics configured
```
❯ docker run -d -p 8999:80 -e "PROMITOR_CONFIGURATION_PATH=default-scrape-configuration.yaml" promitor-scraper 
```

# Configuration
Configuration is done via YAML file that needs to be in the `/config` folder.

```yaml
metrics:
  - name: "queue-size"
    resourceType: "Microsoft.ServiceBus"
  - name: "api-requests-count"
    resourceType: "Microsoft.WebApps"
```

# Powered By
- [YamlDotNet](https://github.com/aaubry/YamlDotNet)
- [Travic CI](https://travis-ci.com/)

# License Information
This is licensed under The MIT License (MIT). Which means that you can use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the web application. But you always need to state that Codit is the original author of this web application.
