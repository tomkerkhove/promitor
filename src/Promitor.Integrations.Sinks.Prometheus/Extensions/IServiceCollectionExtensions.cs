using Microsoft.Extensions.DependencyInjection;
using Prometheus.Client.DependencyInjection;
using Promitor.Core.Metrics.Prometheus.Collectors;
using Promitor.Core.Metrics.Prometheus.Collectors.Interfaces;

namespace Promitor.Agents.Core.Extensions
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