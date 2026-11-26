using System;

namespace Promitor.Core.Scraping.Configuration.Model
{
    public class Aggregation
    {
        public TimeSpan? Interval { get; set; }

        /// <summary>
        ///     How far behind the most recent time grain the reported value should be read from, to tolerate Azure Monitor ingestion delays
        /// </summary>
        public TimeSpan? Offset { get; set; }
    }
}
