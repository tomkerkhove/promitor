using System;

namespace Promitor.Scraper.Validation.Steps
{
    public class ValidationStep
    {
        public void LogMessage(string message)
        {
            Console.WriteLine($"\t\t{message}");
        }
    }
}