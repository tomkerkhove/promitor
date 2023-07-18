using System.Collections.Generic;
using Bogus;
using Promitor.Core;
using Promitor.Core.Metrics;

namespace Promitor.Tests.Unit.Generators
{
    public class ScrapeResultGenerator
    {
        private static readonly Faker bogus = new();

        public static ScrapeResult GenerateFromMetric(MeasuredMetric measuredMetric)
        {
            var subscriptionId = bogus.Name.FirstName();
            var resourceGroupName = bogus.Name.FirstName();
            var instanceName = bogus.Name.FirstName();
            var resourceUri = bogus.Internet.Url();

            return new ScrapeResult(subscriptionId, resourceGroupName, instanceName, resourceUri, new List<MeasuredMetric> {measuredMetric});
        }

        public static ScrapeResult Generate(double metricValue)
        {
            return GenerateFromMetric(MeasuredMetric.CreateWithoutDimensions(metricValue));
        }
    }
}