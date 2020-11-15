---
layout: default
title: Authentication with Microsoft Azure
---

This document describes the Promitor agents, what Microsoft Azure services they are consuming and what the
 minimal required permissions are that every entity requires to be functional.

Here is an overview of our agents and integrations:

| Azure Integration    | Promitor Scraper | Promitor Resource Discovery |
|:---------------------|:----------------:|:---------------------------:|
| Azure Monitor        | ✅               | ❌                         |
| Azure Resource Graph | ❌               | ✅                         |

## Required permissions for Azure Monitor

Authentication with Azure Monitor is fully integrated with Azure AD. In order to
use Promitor, you'll need to [create an Azure AD Application](https://docs.microsoft.com/en-us/azure/active-directory/develop/howto-create-service-principal-portal#create-an-azure-active-directory-application),
that will be used for integrating with the Azure Monitor API.

The following environment variables need to be provided:

- **PROMITOR_AUTH_APPID** - Id of the Azure AD entity to authenticate with
- **PROMITOR_AUTH_APPKEY** - Secret of the Azure AD entity to authenticate with

This information can be found on the newly created AD Application as documented [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/howto-create-service-principal-portal#get-application-id-and-authentication-key).

The entity in the Azure AD needs to have `Monitoring Reader` permission on the
resource group that will be queried. More information can be found [here](https://docs.microsoft.com/en-us/azure/monitoring-and-diagnostics/monitoring-roles-permissions-security).

## Required permissions for Azure Resource Graph

Authentication with Azure Monitor is fully integrated with Azure AD. In order to
use Promitor, you'll need to [create an Azure AD Application](https://docs.microsoft.com/en-us/azure/active-directory/develop/howto-create-service-principal-portal#create-an-azure-active-directory-application),
that will be used for integrating with the Azure Monitor API.

The following environment variables need to be provided:

- **PROMITOR_AUTH_APPID** - Id of the Azure AD entity to authenticate with
- **PROMITOR_AUTH_APPKEY** - Secret of the Azure AD entity to authenticate with

This information can be found on the newly created AD Application as documented [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/howto-create-service-principal-portal#get-application-id-and-authentication-key).

The entity in the Azure AD needs to have `Reader` permission on the
resource group that will be queried. More information can be found [here](https://docs.microsoft.com/en-us/azure/governance/resource-graph/overview#permissions-in-azure-resource-graph).

[&larr; back](/)
