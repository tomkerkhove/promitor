using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Promitor.Tests.Unit.Agents.Core
{
    public class PrometheusRuntimeMetricsTests
    {
        [Fact]
        public async Task Scrape_RuntimeMetrics_PreservesExistingNamesAndValues()
        {
            using var host = new HostBuilder()
                .ConfigureWebHost(webHost => webHost
                    .UseKestrel(options => options.Listen(IPAddress.Loopback, 0))
                    .Configure(app => app.UsePrometheusMetrics("/metrics", NullLogger.Instance)))
                .Build();

            await host.StartAsync();
            try
            {
                var address = host.Services.GetRequiredService<IServer>()
                    .Features.Get<IServerAddressesFeature>().Addresses.Single();
                using var client = new HttpClient();
                var response = await client.GetStringAsync(new Uri(new Uri(address), "/metrics"));
                var lines = response.Split('\n');
                var expectedMetricNames = new[]
                {
                    "promitor_runtime_dotnet_totalmemory",
                    "promitor_runtime_process_virtual_bytes",
                    "promitor_runtime_process_working_set",
                    "promitor_runtime_process_private_bytes",
                    "promitor_runtime_process_num_threads",
                    "promitor_runtime_process_processid",
                    "promitor_runtime_process_start_time_seconds"
                };

                foreach (var metricName in expectedMetricNames)
                {
                    Assert.Contains($"# TYPE {metricName} gauge", response);
                    var sample = Assert.Single(lines, line => line.StartsWith(metricName + " ", StringComparison.Ordinal));
                    Assert.True(double.TryParse(sample.Substring(metricName.Length + 1),
                        NumberStyles.Float, CultureInfo.InvariantCulture, out var value));
                    Assert.True(double.IsFinite(value) && value >= 0, $"Invalid value for {metricName}: {value}");
                }
            }
            finally
            {
                await host.StopAsync();
            }
        }
    }
}
