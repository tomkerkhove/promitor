---
layout: default
title: Authentication with Azure Monitor
---

Here is an overview of how you configure how Promitor integrates with Azure Monitor.

## Authentication with Azure Monitor

Authentication with Azure Monitor is fully integrated with Azure AD. In order to
use Promitor, you'll need to [create an Azure AD Application](https://docs.microsoft.com/en-us/azure/active-directory/develop/howto-create-service-principal-portal#create-an-azure-active-directory-application),
that will be used for integrating with the Azure Monitor API.

The following environment variables need to be provided:

- **PROMITOR_AUTH_APPID** - Id of the Azure AD entity to authenticate with
- **PROMITOR_AUTH_APPKEY** - Secret of the Azure AD entity to authenticate with

This information can be found on the newly created AD Application as documented [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/howto-create-service-principal-portal#get-application-id-and-authentication-key).

The entity in the Azure AD needs to have `Monitoring Reader` permission on the
resource group that will be queried. More information can be found [here](https://docs.microsoft.com/en-us/azure/monitoring-and-diagnostics/monitoring-roles-permissions-security).

[&larr; back](/)
