using System.Collections.Generic;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Scraper.Host.Validation.MetricDefinitions.Interfaces;

namespace Promitor.Scraper.Host.Validation.MetricDefinitions
{
    internal abstract class MetricValidator<TMetricDefinition> : IMetricValidator
        where TMetricDefinition : MetricDefinition
    {
        /// <summary>
        ///     Validates a specific metric definition
        /// </summary>
        /// <param name="metricDefinition">Metric definition to validate</param>
        /// <returns>List of validation errors</returns>
        // ReSharper disable once UnusedMember.Global
        public IEnumerable<string> Validate(MetricDefinition metricDefinition) => Validate(metricDefinition as TMetricDefinition);

        protected abstract IEnumerable<string> Validate(TMetricDefinition metricDefinition);
    }
}
