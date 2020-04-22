using System.Collections.Generic;
using Promitor.Core.Scraping.Configuration.Model.Metrics;

namespace Promitor.Agents.Scraper.Validation.MetricDefinitions.Interfaces
{
    public interface IMetricValidator
    {
        /// <summary>
        ///     Validates a specific metric definition
        /// </summary>
        /// <param name="metricDefinition">Metric definition to validate</param>
        /// <returns>List of validation errors</returns>
        // ReSharper disable once UnusedMember.Global
        IEnumerable<string> Validate(MetricDefinition metricDefinition);
    }
}