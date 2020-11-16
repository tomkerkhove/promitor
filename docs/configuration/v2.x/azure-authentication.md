---
layout: default
title: Authentication with Microsoft Azure
---

This document describes the various agents that Promitor provides, what Microsoft Azure services they are consuming and
 what the minimal required permissions are that every entity requires to be functional.

## Overview

Here is an overview of our Promitor agents and their integrations:

| Azure Integration    | Promitor Scraper | Promitor Resource Discovery |
|:---------------------|:----------------:|:---------------------------:|
| Azure Monitor        | ✅               | ❌                         |
| Azure Resource Graph | ❌               | ✅                         |

Each agent needs an Azure AD identity to authenticate with to Microsoft Azure and needs to be configured with the
 following environment variables:

- **PROMITOR_AUTH_APPID** - Id of the Azure AD entity to authenticate with
- **PROMITOR_AUTH_APPKEY** - Secret of the Azure AD entity to authenticate with

In order to use Promitor, you'll need to [create an Azure AD Application](https://docs.microsoft.com/en-us/azure/active-directory/develop/howto-create-service-principal-portal#create-an-azure-active-directory-application),
that will be used for integrating with the various Microsoft Azure APIs.

> ⚠ For now, we do not support managed identity but this is planned ([issue](https://github.com/tomkerkhove/promitor/issues/444)).

## Permission Overview

### Required permissions for Azure Monitor

Identities that are used to integrate with Azure Monitor need to have `Monitoring Reader` permission on the
subscription, resource group and/or resources that will be queried.

More information can be found [here](https://docs.microsoft.com/en-us/azure/monitoring-and-diagnostics/monitoring-roles-permissions-security).

### Required permissions for Azure Resource Graph

Identities that are used to integrate with Azure Resource Graph need to have `Reader` permission on the
subscription, resource group and/or resources that will be queried.

> ⚠ If you are re-using this identity to integrate with Azure Monitor, make sure to grant the required permissions
 to reflect that as well.

More information can be found [here](https://docs.microsoft.com/en-us/azure/governance/resource-graph/overview#permissions-in-azure-resource-graph).

[&larr; back](/)
