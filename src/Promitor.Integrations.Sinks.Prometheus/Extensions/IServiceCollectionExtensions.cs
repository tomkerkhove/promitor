using Microsoft.Extensions.DependencyInjection;
using Prometheus.Client.DependencyInjection;
using Promitor.Core.Metrics.Interfaces;
using Promitor.Integrations.Sinks.Prometheus.Collectors;

namespace Promitor.Integrations.Sinks.Prometheus.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Use prometheus for writing metrics
        /// </summary>
        public static IServiceCollection AddPrometheusSystemMetrics(this IServiceCollection services)
        {
            services.AddMetricFactory();
            services.AddTransient<ISystemMetricsSink, PrometheusSystemMetricsSink>();

            return services;
        }
    }
}