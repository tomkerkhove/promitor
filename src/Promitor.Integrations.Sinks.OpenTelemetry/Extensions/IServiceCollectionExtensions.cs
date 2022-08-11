using Microsoft.Extensions.DependencyInjection;
using Promitor.Core.Metrics.Prometheus.Collectors.Interfaces;
using Promitor.Integrations.Sinks.OpenTelemetry.Collectors;

namespace Promitor.Agents.Core.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Use OpenTelemetry for writing metrics
        /// </summary>
        public static IServiceCollection AddOpenTelemetrySystemMetrics(this IServiceCollection services)
        {
            services.AddTransient<ISystemMetricsCollector, OpenTelemetrySystemMetricsCollector>();

            return services;
        }
    }
}