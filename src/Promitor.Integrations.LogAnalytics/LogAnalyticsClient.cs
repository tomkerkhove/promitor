using System;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Azure.Monitor.Query;
using GuardNet;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Extensions.Logging;

namespace Promitor.Integrations.LogAnalytics
{
    public class LogAnalyticsClient
    {
        private readonly ILogger _logger;
        public readonly string ColumnNameResult = "result";

        private readonly LogsQueryClient _logsQueryClient;

        public LogAnalyticsClient(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<LogAnalyticsClient>();
            _logsQueryClient = new LogsQueryClient(new DefaultAzureCredential());
        }

        public LogAnalyticsClient(ILoggerFactory loggerFactory, Uri logAnalyticsEndpoint, TokenCredential tokenCredentials)
        {
            _logger = loggerFactory.CreateLogger<LogAnalyticsClient>();
            _logsQueryClient = new LogsQueryClient(logAnalyticsEndpoint, tokenCredentials);
        }

        public async Task<double> RunKustoQueryAsync(string workspaceId, string query, TimeSpan aggregationInterval)
        {
            Guard.NotNullOrWhitespace(workspaceId, nameof(workspaceId));
            Guard.NotNullOrWhitespace(query, nameof(query));
            Guard.For<ArgumentException>(() => aggregationInterval == default);

            var queryResult = await _logsQueryClient.QueryWorkspaceAsync(workspaceId, query, new QueryTimeRange(aggregationInterval, DateTime.UtcNow));

            if (queryResult?.Value?.Table?.Rows == null)
            {
                throw new Exception("Query result cannot be null here");
            }

            if (queryResult.Value.Table.Rows.Count != 1)
            {
                _logger.LogError("Query result length need to be 1, instead it was {number}", queryResult.Value.Table.Rows.Count);
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