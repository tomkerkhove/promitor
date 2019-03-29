using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promitor.Scraper.Host.Models
{
    public class HealthMonitor
    {
        public int subscriptionLimitCount { get; set; }
        private HealthMonitor()
        {
            subscriptionLimitCount = int.MinValue;
        }

        private static Lazy<HealthMonitor> _instance = new Lazy<HealthMonitor>(() => new HealthMonitor());
        public static HealthMonitor Instance { get; } = _instance.Value;

    }
}
