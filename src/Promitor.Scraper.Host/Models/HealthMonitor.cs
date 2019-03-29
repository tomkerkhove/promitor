using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promitor.Scraper.Host.Models
{
    public class HealthMonitor
    {
        public int subscriptionLimitCount { get; set; }
        public HealthMonitor()
        {
            subscriptionLimitCount = int.MinValue;
        }

    }
}
