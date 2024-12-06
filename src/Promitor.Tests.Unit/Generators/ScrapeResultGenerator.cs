using System.Collections.Generic;
using Bogus;
using Promitor.Core;
using Promitor.Core.Metrics;

namespace Promitor.Tests.Unit.Generators
{
    public class ScrapeResultGenerator
    {
        private static readonly Faker Bogus = new();

        public static ScrapeResult GenerateFromMetric(MeasuredMetric measuredMetric)
        {
            var subscriptionId = Bogus.Name.FirstName();
            var resourceGroupName = Bogus.Name.FirstName();
            var instanceName = Bogus.Name.FirstName();
            var resourceUri = Bogus.Internet.Url();

            return new ScrapeResult(subscriptionId, resourceGroupName, instanceName, resourceUri, new List<MeasuredMetric> {measuredMetric});
        }

        public static ScrapeResult Generate(double metricValue)
        {
            return GenerateFromMetric(MeasuredMetric.CreateWithoutDimensions(metricValue));
        }
    }
}