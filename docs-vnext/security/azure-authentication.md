# Authentication with Microsoft Azure

This document describes the various agents that Promitor provides, what Microsoft Azure services they are consuming and
 what the minimal required permissions are that every entity requires to be functional.

## Overview

Here is an overview of our Promitor agents and their integrations:

| Azure Integration    | Promitor Scraper | Promitor Resource Discovery |
|:---------------------|:----------------:|:---------------------------:|
| Azure Monitor        | ✅               | ❌                         |
| Azure Resource Graph | ❌               | ✅                         |

Each agent needs an Azure AD identity to authenticate with to Microsoft Azure.

In order to achieve this, you'll need to [create an Azure AD Application](https://docs.microsoft.com/en-us/azure/active-directory/develop/howto-create-service-principal-portal#create-an-azure-active-directory-application),

## Supported Authentication Mechanisms

Our agents provide the following authentication mechanisms:

- **Service principle** - Use application id & secret of the Azure AD entity that has been pre-created to authenticate with
- **Managed Identity** - Use zero-secret authentication by letting Microsoft handle the authentication for you ([docs](https://docs.microsoft.com/en-us/azure/active-directory/managed-identities-azure-resources/overview))

For details how to configure the authentication, we recommend reading our agent configuration documentation.

### Service Principle Authentication

![Availability Badge](https://img.shields.io/badge/Available%20Starting-v0.1-green.svg)

Every agent needs to be configured with the following environment variables:

- **PROMITOR_AUTH_APPKEY** - Secret of the Azure AD entity to authenticate with

### Managed Identity Authentication

![Availability Badge](https://img.shields.io/badge/Available%20Starting-v2.2-green.svg)

When using Managed Identity, you can use one of the following scenarios:

- **System-assigned Managed Identity** - Use the identity of the Azure resource on which it runs and let Azure handle
 the authentication.
- **User-assigned Managed Identity** - Use a pre-created Azure AD identity but let Azure handle the authentication for you

> ⚠ In order to use managed identity, your Kubernetes cluster must be hosted on Microsoft Azure to leverage this.

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
