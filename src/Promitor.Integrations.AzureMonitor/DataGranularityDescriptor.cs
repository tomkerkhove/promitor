using System;

namespace Promitor.Integrations.AzureMonitor
{
    public class DataGranularityDescriptor
    {
        public static DataGranularityDescriptor Default => new DataGranularityDescriptor { DataGranularity = DataGranularity.Specified, SpecifiedTimeSpan = TimeSpan.FromMinutes(5) };

        public DataGranularity DataGranularity { get; set; }
        public TimeSpan SpecifiedTimeSpan { get; set; }

        public DataGranularityDescriptor()
        {
            this.DataGranularity = DataGranularity.NotSpecified;
            this.SpecifiedTimeSpan = TimeSpan.Zero;
        }
    }
}