using System.Collections.Generic;
using Bogus;
using Promitor.Integrations.Sinks.Atlassian.Statuspage.Configuration;
using Promitor.Tests.Unit.Stubs;

namespace Promitor.Tests.Unit.Generators.Config
{
    internal static class BogusAtlassianStatuspageMetricSinkConfigurationGenerator
    {
        internal static AtlassianStatusPageSinkConfiguration Generate(string pageId = null, string systemMetricId = null, string promitorMetricName = null)
        {
            var systemMetricMapping = new Faker<SystemMetricMapping>()
                .RuleFor(config => config.Id, fake => systemMetricId ?? fake.Name.FirstName())
                .RuleFor(config => config.PromitorMetricName, fake => promitorMetricName ?? fake.Name.FirstName())
                .Generate();

            var sinkConfiguration = new Faker<AtlassianStatusPageSinkConfiguration>()
                .RuleFor(config => config.PageId, fake => pageId ?? fake.Name.FirstName())
                .RuleFor(config => config.SystemMetricMapping, _ => new List<SystemMetricMapping> { systemMetricMapping })
                .Generate();

            return sinkConfiguration;
        }

        internal static OptionsMonitorStub<AtlassianStatusPageSinkConfiguration> GetSinkConfiguration(string pageId = null, string systemMetricId = null, string promitorMetricName = null)
        {
            var sinkConfiguration = BogusAtlassianStatuspageMetricSinkConfigurationGenerator.Generate(pageId, systemMetricId, promitorMetricName);

            return new OptionsMonitorStub<AtlassianStatusPageSinkConfiguration>(sinkConfiguration);
        }
    }
}