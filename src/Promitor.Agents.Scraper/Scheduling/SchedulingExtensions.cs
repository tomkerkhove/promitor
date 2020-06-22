using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Promitor.Agents.Scraper;
using Promitor.Agents.Scraper.Discovery;
using Promitor.Core.Scraping.Configuration.Providers.Interfaces;
using Promitor.Core.Scraping.Factories;
using Promitor.Agents.Scraper.Scheduling;
using Promitor.Core.Contracts;
using Promitor.Core.Metrics;
using Promitor.Core.Metrics.Sinks;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Interfaces;
using Promitor.Integrations.AzureMonitor.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    // ReSharper disable once InconsistentNaming
    public static class SchedulingExtensions
    {
        /// <summary>
        ///     Defines to use the cron scheduler
        /// </summary>
        /// <param name="services">Collections of services in application</param>
        public static IServiceCollection ScheduleMetricScraping(this IServiceCollection services)
        {
            var serviceProviderToCreateJobsWith = services.BuildServiceProvider();
            var metricsProvider = serviceProviderToCreateJobsWith.GetService<IMetricsDeclarationProvider>();
            var metrics = metricsProvider.Get(applyDefaults: true);

            var loggerFactory = serviceProviderToCreateJobsWith.GetService<ILoggerFactory>();
            var metricSinkWriter = serviceProviderToCreateJobsWith.GetRequiredService<MetricSinkWriter>();
            var azureMonitorLoggingConfiguration = serviceProviderToCreateJobsWith.GetService<IOptions<AzureMonitorLoggingConfiguration>>();
            var configuration = serviceProviderToCreateJobsWith.GetService<IConfiguration>();
            var runtimeMetricCollector = serviceProviderToCreateJobsWith.GetService<IRuntimeMetricsCollector>();
            var azureMonitorClientFactory = serviceProviderToCreateJobsWith.GetRequiredService<AzureMonitorClientFactory>();
            var startupLogger = loggerFactory.CreateLogger<Startup>();
            foreach (var metric in metrics.Metrics)
            {
                if (metric.ResourceCollections?.Any() == true)
                {
                    foreach (var resourceCollection in metric.ResourceCollections)
                    {
                        ScheduleResourceCollectionScraping(resourceCollection, metrics.AzureMetadata, metric, azureMonitorClientFactory, metricSinkWriter, runtimeMetricCollector, configuration, azureMonitorLoggingConfiguration, loggerFactory, startupLogger, services);
                    }
                }

                if (metric.Resources != null)
                {
                    foreach (var resource in metric.Resources)
                    {
                        ScheduleResourceScraping(services, resource, metrics, azureMonitorClientFactory, metricSinkWriter, runtimeMetricCollector, configuration, azureMonitorLoggingConfiguration, loggerFactory, metric, startupLogger);
                    }
                }
            }

            return services;
        }

        private static void ScheduleResourceScraping(IServiceCollection services, IAzureResourceDefinition resource, MetricsDeclaration metrics, AzureMonitorClientFactory azureMonitorClientFactory, MetricSinkWriter metricSinkWriter, IRuntimeMetricsCollector runtimeMetricCollector, IConfiguration configuration, IOptions<AzureMonitorLoggingConfiguration> azureMonitorLoggingConfiguration, ILoggerFactory loggerFactory, MetricDefinition metric, ILogger<Startup> logger)
        {
            var resourceSubscriptionId = string.IsNullOrWhiteSpace(resource.SubscriptionId) ? metrics.AzureMetadata.SubscriptionId : resource.SubscriptionId;
            var azureMonitorClient = azureMonitorClientFactory.CreateIfNotExists(metrics.AzureMetadata.Cloud, metrics.AzureMetadata.TenantId, resourceSubscriptionId, metricSinkWriter, runtimeMetricCollector, configuration, azureMonitorLoggingConfiguration, loggerFactory);
            var scrapeDefinition = metric.CreateScrapeDefinition(resource, metrics.AzureMetadata);
            var jobName = GenerateResourceScrapingJobName(scrapeDefinition, resource);

            services.AddScheduler(builder =>
            {
                builder.AddJob(jobServices =>
                    {
                        return new ResourceScrapingJob(jobName, scrapeDefinition,
                            metricSinkWriter,
                            jobServices.GetService<IPrometheusMetricWriter>(),
                            jobServices.GetService<MetricScraperFactory>(),
                            azureMonitorClient,
                            jobServices.GetService<ILogger<ResourceScrapingJob>>());
                    }, schedulerOptions =>
                    {
                        schedulerOptions.CronSchedule = scrapeDefinition.Scraping.Schedule;
                        schedulerOptions.RunImmediately = true;
                    },
                    jobName: jobName);
                builder.UnobservedTaskExceptionHandler = (sender, exceptionEventArgs) => UnobservedJobHandlerHandler(jobName, exceptionEventArgs, services);
            });

            logger.LogInformation("Scheduled scraping job {JobName} for resource {Resource} which will be reported as metric {MetricName}", jobName, scrapeDefinition.Resource.UniqueName, scrapeDefinition.PrometheusMetricDefinition?.Name);
        }

        private static void ScheduleResourceCollectionScraping(AzureResourceCollection resourceCollection, AzureMetadata azureMetadata, MetricDefinition metricDefinition, AzureMonitorClientFactory azureMonitorClientFactory, MetricSinkWriter metricSinkWriter, IRuntimeMetricsCollector runtimeMetricCollector, IConfiguration configuration, IOptions<AzureMonitorLoggingConfiguration> azureMonitorLoggingConfiguration, ILoggerFactory loggerFactory, ILogger<Startup> logger, IServiceCollection services)
        {
            var jobName = GenerateResourceCollectionScrapingJobName(metricDefinition.PrometheusMetricDefinition?.Name, resourceCollection.Name);
            services.AddScheduler(builder =>
            {
                builder.AddJob(jobServices =>
                {
                    return new ResourceCollectionScrapingJob(jobName, resourceCollection.Name, azureMetadata, metricDefinition,
                        jobServices.GetService<ResourceDiscoveryRepository>(),
                        metricSinkWriter,
                        jobServices.GetService<IPrometheusMetricWriter>(),
                        jobServices.GetService<MetricScraperFactory>(),
                        azureMonitorClientFactory,
                        runtimeMetricCollector,
                        configuration,
                        azureMonitorLoggingConfiguration,
                        loggerFactory,
                        jobServices.GetService<ILogger<ResourceCollectionScrapingJob>>());
                }, schedulerOptions =>
                {
                    schedulerOptions.CronSchedule = metricDefinition.Scraping.Schedule;
                    schedulerOptions.RunImmediately = true;
                },
                    jobName: jobName);
                builder.UnobservedTaskExceptionHandler = (sender, exceptionEventArgs) => UnobservedJobHandlerHandler(jobName, exceptionEventArgs, services);
            });

            logger.LogInformation("Scheduled scraping job {JobName} for resource collection {ResourceCollection} which will be reported as metric {MetricName}", jobName, resourceCollection.Name, metricDefinition.PrometheusMetricDefinition?.Name);
        }

        private static string GenerateResourceScrapingJobName(ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, IAzureResourceDefinition resource)
        {
            var jobNameBuilder = new StringBuilder();
            jobNameBuilder.Append(scrapeDefinition.SubscriptionId);
            jobNameBuilder.Append("-");
            jobNameBuilder.Append(scrapeDefinition.ResourceGroupName);
            jobNameBuilder.Append("-");
            jobNameBuilder.Append(scrapeDefinition.PrometheusMetricDefinition.Name);
            jobNameBuilder.Append("-");
            jobNameBuilder.Append(resource.UniqueName);
            jobNameBuilder.Append("-");
            jobNameBuilder.Append(Guid.NewGuid().ToString());

            return jobNameBuilder.ToString();
        }

        private static string GenerateResourceCollectionScrapingJobName(string metricName, string resourceCollectionName)
        {
            var jobNameBuilder = new StringBuilder();
            jobNameBuilder.Append(metricName);
            jobNameBuilder.Append("-");
            jobNameBuilder.Append(resourceCollectionName);
            jobNameBuilder.Append("-");
            jobNameBuilder.Append(Guid.NewGuid().ToString());

            return jobNameBuilder.ToString();
        }

        // ReSharper disable once UnusedParameter.Local
        private static void UnobservedJobHandlerHandler(string jobName, UnobservedTaskExceptionEventArgs exceptionEventArgs, IServiceCollection services)
        {
            var logger = services.FirstOrDefault(service => service.ServiceType == typeof(ILogger));
            var loggerInstance = (ILogger) logger?.ImplementationInstance;

            loggerInstance?.LogCritical(exceptionEventArgs.Exception, "Unhandled exception in job {JobName}", jobName);

            exceptionEventArgs.SetObserved();
        }
    }
}