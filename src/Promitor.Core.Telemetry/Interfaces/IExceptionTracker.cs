using System;

namespace Promitor.Core.Telemetry.Interfaces
{
    public interface IExceptionTracker
    {
        void Track(Exception exception);
    }
}