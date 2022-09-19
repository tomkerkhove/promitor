using Microsoft.Extensions.DependencyInjection;
using Promitor.Core.Metrics.Interfaces;
using Promitor.Integrations.Sinks.OpenTelemetry.Collectors;

namespace Promitor.Integrations.Sinks.OpenTelemetry.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Use OpenTelemetry for writing metrics
        /// </summary>
        public static IServiceCollection AddOpenTelemetrySystemMetrics(this IServiceCollection services)
        {
            services.AddTransient<ISystemMetricsSink, OpenTelemetrySystemMetricsSink>();

            return services;
        }
    }
}