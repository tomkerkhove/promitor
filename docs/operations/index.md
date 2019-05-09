---
layout: default
title: Operating Promitor
---

Here is an overview of how you can operate Promitor. 

# Azure Resource Manager API Consumption & Throttling
Promitor exposes runtime metrics to provide insights on the API consumption of Azure Resource Manager API:

- `promitor_ratelimit_arm_subscription` - Indication how many calls are still available before Azure Resource Manager is going to throttle us.
Metric provides following labels:
    - `tenant_id` - _Id of the tenant that is being interacted with_
    - `subscription_id` - _Id of the subscription that is being interacted with_
    - `app_id` - _Id of the application that is being used to interact with API_

# Health
Promitor provides a basic health endpoint that indicates the state of the scraper.

Health endpoints can be useful for monitoring the scraper, running sanity tests after deployments or use it for sending liveness / health probes.

## Consuming the health endpoint
You can check the status with a simple `GET`:
```
❯ curl -i -X GET "http://<uri>/api/v1/health"
```

Health is currently indicated via the HTTP response status:
- `200 OK` - The scraper is healthy
- `503 Service Unavailable` - The scraper is unhealthy

In the future, the endpoint will be more advanced by giving detailed status on dependencies as well.

[&larr; back](/)
