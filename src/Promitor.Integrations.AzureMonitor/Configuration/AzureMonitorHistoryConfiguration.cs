using System;
namespace Promitor.Integrations.AzureMonitor.Configuration
{
    public class AzureMonitorHistoryConfiguration
    {
        public int StartingFromInHours { get; set; } = 12;
        public TimeSpan? HistoryStartingFromOffset { get; set; } = null;

        public TimeSpan ResolveEffectiveStartingFrom()
        {
            return HistoryStartingFromOffset ?? TimeSpan.FromHours(StartingFromInHours);
        }
    }
}