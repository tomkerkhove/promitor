using System;

namespace Promitor.Scraper.Telemetry.Interfaces
{
    public interface IExceptionTracker
    {
        void Track(Exception exception);
    }
}