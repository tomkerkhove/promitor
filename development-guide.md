# Promitor Development Guide

This guide explains the layout of the Promitor repository, along with the tools
and technologies required to develop Promitor.

## Repository Structure

The promitor repository is made up of a number of different directories:

- `/` - contains documentation about the repository including the main README file
  and some configuration files for various tools used for development.
- `/.github` - contains github specific files including issue and PR templates.
- `/.vscode` - contains shared configuration files for [VS Code](https://code.visualstudio.com/).
- `/build` - contains the configuration files for Promitor's CI process.
- `/charts` - contains Promitor's [Helm](https://helm.sh/) chart.
- `/deploy` - contains the automation that is being used to manage Promitor, such
  as the automated updates concerning new Docker images in a pull request.
- `/docs` - contains the source code (mainly markdown files) for building the
  <https://promitor.io>.
- `/src` - contains the .NET source code for the Promitor application.

## Helm Chart

The code for Promitor's Helm chart can be found in [charts/promitor-agent-scraper](charts/promitor-agent-scraper).

To work on the Helm chart, you need the following tools:

- [Helm](https://helm.sh).
- A working Kubernetes cluster. For local development you can use [Docker Desktop](https://www.docker.com/products/docker-desktop)
  which has an option to install a local cluster for you, or a tool like [Minikube](https://github.com/kubernetes/minikube).

The chart's README contains information about how to install the chart in your Kubernetes
cluster. For local development, you can substitute the relative path to the chart
instead of using the chart name, for example:

```shell
helm install --name promitor-agent-scraper ./charts/promitor-agent-scraper \
  --set azureAuthentication.appId='<azure-ad-app-id>' \
  ...
```

Please follow Helm's [Best Practices](https://helm.sh/docs/chart_best_practices/#the-chart-best-practices-guide)
when developing the chart.

## Documentation

The documentation for Promitor is stored in `/docs`. When adding new functionality
to Promitor or modifying existing functionality, please add associated documentation.
Information about how to build and run the documentation locally can be found in
the [README](docs/README.md) for the documentation.

## .NET Development

Promitor is written in C# using .NET Core. To make changes to Promitor you need the
following tools:

- [.NET Core SDK](https://dotnet.microsoft.com/download).
- Visual Studio 2017+, or an alternative editor like VS Code.

The C# code for Promitor can be found in the `/src` folder in the repository. The
Promitor code is split into multiple projects by functionality, with the following
projects worth highlighting:

- Promitor.Agents.Scraper - contains the main console application that runs Promitor.
- Promitor.Tests.Unit - contains the XUnit unit tests.
- Promitor.Docker - contains a Visual Studio Docker project for running Promitor
  during development.

### Visual Studio

If you are using Visual Studio, the solution file for Promitor is [src/Promitor.sln](src/Promitor.sln).

### Running Tests

To run the unit tests for Promitor, open a terminal and navigate to `src/Promitor.Tests.Unit`,
and run the following command:

```shell
dotnet test
```

If you want to perform TDD, you can use the `dotnet watch` command to watch for
changes and re-run the tests:

```shell
dotnet watch test
```

Alternatively, you can use the built-in tests runners in Visual Studio or whatever
tool you are using to edit the code.

### Running Promitor

To run Promitor, edit [src/docker-compose.override.yml](src/docker-compose.override.yml)
and set the following environment variables:

- `PROMITOR_AUTH_APPID` - the client Id of the service principal used to access the
  Azure Monitor API.
- `PROMITOR_AUTH_APPKEY` - your service principal secret.

Next, edit [src/metrics.yaml](src/metrics.yaml) and set the following
keys:

- `azureMetadata.tenantId` - your Azure tenant Id.
- `azureMetadata.subscriptionId` - your Azure subscription Id.
- `azureMetadata.resourceGroupName` - the default resource group to use if none
  is specified for a metric.

Configure at least one scraper under the `metrics` section and finally, run the
Promitor.Docker project.

You can find more information about how to configure Promitor [here](https://promitor.io/configuration/v1.x/metrics/).

**NOTE:** Please make sure not to commit your changes to `docker-compose.override.yml`
or `metrics.yaml`. If you do, you may end up publishing your Azure credentials
by accident.

## Docker

To build the Docker image, run the following command from the `/src` directory:

```shell
docker build . --file .\Promitor.Agents.Scraper\Dockerfile --tag promitor --no-cache
```
