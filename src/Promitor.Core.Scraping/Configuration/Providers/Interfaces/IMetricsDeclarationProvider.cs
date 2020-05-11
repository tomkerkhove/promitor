using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Serialization;

namespace Promitor.Core.Scraping.Configuration.Providers.Interfaces
{
    public interface IMetricsDeclarationProvider
    {
        /// <summary>
        ///     Gets the configured metrics declaration
        /// </summary>
        /// <param name="applyDefaults">
        ///     <c>true</c> if the provider should apply default values from top-level
        ///     configuration elements to metrics where those values aren't specified. <c>false</c> otherwise
        /// </param>
        /// <param name="errorReporter">Used to report errors during the deserialization process.</param>
        MetricsDeclaration Get(bool applyDefaults = false, IErrorReporter errorReporter = null);

        /// <summary>
        ///     Get metric definition for a given metric
        /// </summary>
        /// <param name="metricName">Name of metric</param>
        /// <param name="applyDefaults">
        ///     <c>true</c> if the provider should apply default values from top-level
        ///     configuration elements to metrics where those values aren't specified. <c>false</c> otherwise
        /// </param>
        /// <param name="errorReporter">Used to report errors during the deserialization process.</param>
        /// <returns></returns>
        MetricDefinition GetMetricDefinition(string metricName, bool applyDefaults = false, IErrorReporter errorReporter = null);

        /// <summary>
        ///     Get Prometheus definition for a given metric
        /// </summary>
        /// <param name="metricName">Name of metric</param>
        /// <param name="applyDefaults">
        ///     <c>true</c> if the provider should apply default values from top-level
        ///     configuration elements to metrics where those values aren't specified. <c>false</c> otherwise
        /// </param>
        /// <param name="errorReporter">Used to report errors during the deserialization process.</param>
        /// <returns></returns>
        PrometheusMetricDefinition GetPrometheusDefinition(string metricName, bool applyDefaults = false, IErrorReporter errorReporter = null);

        /// <summary>
        ///     Gets the serialized metrics declaration
        /// </summary>
        string ReadRawDeclaration();
    }
}