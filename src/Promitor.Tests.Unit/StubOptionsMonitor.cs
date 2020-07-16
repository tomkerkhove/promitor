using System;
using Microsoft.Extensions.Options;

namespace Promitor.Tests.Unit
{
    public class StubOptionsMonitor<TOptions> : IOptionsMonitor<TOptions>
    {
        public StubOptionsMonitor(TOptions currentValue)
        {
            CurrentValue = currentValue;
        }

        public TOptions Get(string name)
        {
            return CurrentValue;
        }

        public IDisposable OnChange(Action<TOptions, string> listener)
        {
            throw new NotImplementedException();
        }

        public TOptions CurrentValue { get; }
    }
}
