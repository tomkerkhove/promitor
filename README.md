# Promitor - Azure Monitor Scraper for Prometheus 
[![License](https://img.shields.io/github/license/mashape/apistatus.svg)](./LICENSE)[![Build Status](https://travis-ci.com/tomkerkhove/promitor.svg?token=GsSXSXe5xF8ZdYK5qExq&branch=master)](https://travis-ci.com/tomkerkhove/promitor)

Azure Monitor Scraper for Prometheus

----------------------------
:rotating_light: This is under active development and subject to change.
----------------------------

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
