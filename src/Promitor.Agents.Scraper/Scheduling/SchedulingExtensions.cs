using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Promitor.Agents.Core.Observability;
using Promitor.Agents.Scraper;
using Promitor.Agents.Scraper.Discovery;
using Promitor.Agents.Scraper.Scheduling;
using Promitor.Core.Scraping.Configuration.Providers.Interfaces;
using Promitor.Core.Scraping.Factories;
using Promitor.Core.Metrics.Prometheus.Collectors.Interfaces;
using Promitor.Core.Metrics.Sinks;
using Promitor.Core.Scraping.Configuration.Model;
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
            var metricsProvider = serviceProviderToCreateJobsWith.GetRequiredService<IMetricsDeclarationProvider>();
            var allMetrics = metricsProvider.Get(applyDefaults: true);

            // this is relied on as non-null down the stack
            var loggerFactory = serviceProviderToCreateJobsWith.GetRequiredService<ILoggerFactory>();
            
            var metricSinkWriter = serviceProviderToCreateJobsWith.GetRequiredService<MetricSinkWriter>();
            var azureMonitorLoggingConfiguration = serviceProviderToCreateJobsWith.GetService<IOptions<AzureMonitorLoggingConfiguration>>();
            var resourceMetricDefinitionMemoryCache = serviceProviderToCreateJobsWith.GetService<IMemoryCache>();
            var configuration = serviceProviderToCreateJobsWith.GetService<IConfiguration>();
            var runtimeMetricCollector = serviceProviderToCreateJobsWith.GetService<IAzureScrapingPrometheusMetricsCollector>();
            var azureMonitorClientFactory = serviceProviderToCreateJobsWith.GetRequiredService<AzureMonitorClientFactory>();
            var startupLogger = loggerFactory.CreateLogger<Startup>();
            var scrapingTaskMutex = serviceProviderToCreateJobsWith.GetService<IScrapingMutex>();
            
            var metricsGroupedByScrapingInterval = GroupMetricsByScrapingInterval(allMetrics);

            foreach (var metricsForScrapingInterval in metricsGroupedByScrapingInterval)
            {
                ScheduleResourcesScraping(metricsForScrapingInterval, metricSinkWriter, azureMonitorClientFactory, runtimeMetricCollector, resourceMetricDefinitionMemoryCache, 
                    scrapingTaskMutex, configuration, azureMonitorLoggingConfiguration, loggerFactory, startupLogger, services);
            }

            return services;
        }

        private static ICollection<MetricsDeclaration> GroupMetricsByScrapingInterval(MetricsDeclaration allMetrics)
        {
            var metricsGroupings = new Dictionary<Scraping, MetricsDeclaration>();

            foreach (var metric in allMetrics.Metrics)
            {
                if (string.IsNullOrWhiteSpace(metric?.Scraping?.Schedule))
                    throw new InvalidOperationException($"Metric \"{metric?.AzureMetricConfiguration?.MetricName}\" is missing scraping schedule.");
                
                if (!metricsGroupings.TryGetValue(metric.Scraping, out var metricsGroup))
                {
                    metricsGroup = new MetricsDeclaration
                    {
                        AzureMetadata = allMetrics.AzureMetadata,
                        MetricDefaults = allMetrics.MetricDefaults
                    };
                    metricsGroupings.Add(metric.Scraping, metricsGroup);
                }
                
                metricsGroup.Metrics.Add(metric);
            }

            return metricsGroupings.Values;
        }

        private static void ScheduleResourcesScraping(MetricsDeclaration metricsDeclaration,
            MetricSinkWriter metricSinkWriter,
            AzureMonitorClientFactory azureMonitorClientFactory,
            IAzureScrapingPrometheusMetricsCollector azureScrapingPrometheusMetricsCollector,
            IMemoryCache resourceMetricDefinitionMemoryCache,
            IScrapingMutex scrapingTaskMutex,
            IConfiguration configuration,
            IOptions<AzureMonitorLoggingConfiguration> azureMonitorLoggingConfiguration,
            ILoggerFactory loggerFactory,
            ILogger<Startup> logger,
            IServiceCollection services)
        {
            var jobName = GenerateResourceScrapingJobName(metricsDeclaration, logger);
            
            services.AddScheduler(builder =>
            {
                builder.AddJob(jobServices =>
                        new ResourcesScrapingJob(jobName,
                            metricsDeclaration,
                            jobServices.GetService<ResourceDiscoveryRepository>(),
                            metricSinkWriter,
                            jobServices.GetService<MetricScraperFactory>(),
                            azureMonitorClientFactory,
                            azureScrapingPrometheusMetricsCollector,
                            resourceMetricDefinitionMemoryCache,
                            scrapingTaskMutex,
                            configuration,
                            azureMonitorLoggingConfiguration,
                            loggerFactory,
                            jobServices.GetService<ILogger<ResourcesScrapingJob>>()),
                    schedulerOptions =>
                    {
                        schedulerOptions.CronSchedule = metricsDeclaration.Metrics[0].Scraping.Schedule;
                        schedulerOptions.RunImmediately = true;
                    },
                    jobName: jobName);
                builder.UnobservedTaskExceptionHandler = (_, exceptionEventArgs) => BackgroundJobMonitor.HandleException(jobName, exceptionEventArgs, services);
            });
            logger.LogInformation("Scheduled scraping job {JobName}.", jobName);
        }

        private static string GenerateResourceScrapingJobName(MetricsDeclaration metricsDeclaration, ILogger<Startup> logger)
        {
            string scheduleDescription;
            try
            {
                scheduleDescription = CronExpressionDescriptor.ExpressionDescriptor.GetDescription(metricsDeclaration.Metrics[0].Scraping.Schedule);
                scheduleDescription = scheduleDescription.Replace(", ", "_").Replace(",", "_").Replace(" ", "_");
            }
            catch (Exception ex)
            {
                var metricName = metricsDeclaration.Metrics[0].AzureMetricConfiguration.MetricName;
                logger.LogError(ex, $"Failure to create job name for scraping resources for list of metrics which includes {metricName}.");
                scheduleDescription = $"UnparsedScheduleForMetricsIncluding{metricName}";
            }

            var metricsCount = metricsDeclaration.Metrics.Count;
            var uniqueness = Guid.NewGuid().ToString("N");

            return $"PromitorScraper-{metricsCount}Metrics-{scheduleDescription}-{uniqueness}";
        }
    }
}