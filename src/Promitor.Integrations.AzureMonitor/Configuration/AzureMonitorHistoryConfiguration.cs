using System;
namespace Promitor.Integrations.AzureMonitor.Configuration
{
    public class AzureMonitorHistoryConfiguration
    {
        public int StartingFromInHours { get; set; } = 12;
        public TimeSpan? StartingFromOffset { get; set; } = null;

        public TimeSpan ResolveEffectiveStartingFrom()
        {
            return StartingFromOffset ?? TimeSpan.FromHours(StartingFromInHours);
        }
    }
}