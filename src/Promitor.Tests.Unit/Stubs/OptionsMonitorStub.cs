using System;
using Microsoft.Extensions.Options;

namespace Promitor.Tests.Unit.Stubs
{
    public class OptionsMonitorStub<TConfig> : IOptionsMonitor<TConfig>
    {
        public OptionsMonitorStub(TConfig currentValue)
        {
            CurrentValue = currentValue;
        }

        public TConfig Get(string name)
        {
            return CurrentValue;
        }

        public IDisposable OnChange(Action<TConfig, string> listener)
        {
            return null;
        }

        public TConfig CurrentValue { get; set; }
    }
}