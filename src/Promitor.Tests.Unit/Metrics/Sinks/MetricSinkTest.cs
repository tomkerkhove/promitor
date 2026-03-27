using System.Collections.Generic;
using System.ComponentModel;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Mapping;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Tests.Unit.Builders.Metrics.v1;
using Promitor.Tests.Unit.Stubs;

namespace Promitor.Tests.Unit.Metrics.Sinks
{
    [Category("Unit")]
    public class MetricSinkTest : UnitTest
    {
        protected MetricsDeclarationProviderStub CreateMetricsDeclarationProvider(string metricName,  Dictionary<string, string> labels = null, Dictionary<string, string> defaultLabels = null)
        {
            var mapper = new V1ConfigurationMapper();
            var metricBuilder = MetricsDeclarationBuilder.WithMetadata();

            if (defaultLabels != null)
            {
                var defaults = new MetricDefaultsV1
                {
                    Labels = defaultLabels
                };
                
                metricBuilder.WithDefaults(defaults);
            }

            var rawDeclaration = metricBuilder.WithServiceBusMetric(metricName, labels: labels)
                                                    .Build(mapper);

            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, mapper);
            return metricsDeclarationProvider;
        }
    }
}
