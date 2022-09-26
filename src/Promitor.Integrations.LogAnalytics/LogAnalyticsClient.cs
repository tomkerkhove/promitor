using System;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Azure.Monitor.Query;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Extensions.Logging;

namespace Promitor.Integrations.LogAnalytics
{
    public class LogAnalyticsClient
    {
        private readonly ILogger _logger;
        public readonly int TimeRangeQueryInHours = 12;
        public readonly string ColumnNameResult = "result";
        private readonly Uri _defaultEndpoint = new Uri("https://api.loganalytics.io");
        private readonly Uri _govEndpoint = new Uri("https://api.loganalytics.us");

        private readonly LogsQueryClient _logsQueryClient;

        public LogAnalyticsClient(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<LogAnalyticsClient>();
            _logsQueryClient = new LogsQueryClient(new DefaultAzureCredential());
        }

        public LogAnalyticsClient(ILoggerFactory loggerFactory, AzureEnvironment azureEnvironment, TokenCredential tokenCredentials)
        {
            _logger = loggerFactory.CreateLogger<LogAnalyticsClient>();
            var uri = _defaultEndpoint;
            if (azureEnvironment.Name == nameof(AzureEnvironment.AzureUSGovernment))
                uri = _govEndpoint;

            _logsQueryClient = new LogsQueryClient(uri, tokenCredentials);
        }

        public async Task<double> QueryDouble(string workspaceId, string query)
        {
            var queryResult = await _logsQueryClient.QueryWorkspaceAsync(workspaceId, query, new QueryTimeRange(TimeSpan.FromHours(TimeRangeQueryInHours), DateTime.UtcNow));
            if (queryResult.Value.Table.Rows.Count != 1)
            {
                throw new Exception("Query result length need to be 1");
            }

            var result = queryResult.Value.Table.Rows[0].GetDouble(ColumnNameResult);
            if (result is null)
            {
                throw new Exception("Cannot parse query result to double value");
            }

            return result.Value;
        }
    }
}