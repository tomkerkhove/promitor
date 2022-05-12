<!-- markdownlint-disable -->
<!-- For new scrapers, make sure to follow https://github.com/tomkerkhove/promitor/blob/master/adding-a-new-scraper.md

When implementing a new scraper; these tasks are completed:
- [ ] Implement configuration
- [ ] Implement validation
- [ ] Implement scraping
- [ ] Implement resource discovery
- [ ] Provide unit tests
- [ ] Test end-to-end
- [ ] Document scraper (see https://github.com/promitor/docs/blob/main/CONTRIBUTING.md#documenting-a-new-scraper)
- [ ] Add entry to changelog (see https://github.com/tomkerkhove/promitor/blob/master/CONTRIBUTING.md#changelog)

**Metrics output:**
```
# HELP azure_network_gateway_count_ingress_package_drop Total count of ingress package drops on an Azure network gateway
# TYPE azure_network_gateway_count_ingress_package_drop gauge
azure_network_gateway_count_ingress_package_drop{resource_group="RG",subscription_id="SUB",resource_uri="subscriptions/SUB/resourceGroups/RG/providers/Microsoft.Network/virtualNetworkGateways/Azure-Tele-Gateway",instance_name="Azure-Tele-Gateway"} 19.4 1599219001456
# HELP promitor_ratelimit_arm Indication how many calls are still available before Azure Resource Manager is going to throttle us.
# TYPE promitor_ratelimit_arm gauge
promitor_ratelimit_arm{tenant_id="T",subscription_id="SUB",app_id="APP"} 11996 1599219001431
```

**Discovery output:**
```json
[{"$type":"Promitor.Core.Contracts.ResourceTypes.NetworkGatewayResourceDefinition, Promitor.Core.Contracts","NetworkGatewayName":"Azure-Tele-Gateway","ResourceType":"NetworkGateway","SubscriptionId":"SUB","ResourceGroupName":"RG","ResourceName":"Azure-Tele-Gateway","UniqueName":"Azure-Tele-Gateway"}]
```

 -->

Fixes #

<!-- markdownlint-enable -->
