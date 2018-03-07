using System.Threading.Tasks;

namespace Promitor.Scraper.Validation
{
    public class ScrapingScheduleValidation : IValidation
    {
        public Task ValidateAsync()
        {
            return Task.CompletedTask;
        }
    }
    public class ScrapingSchedule2Validation : IValidation
    {
        public Task ValidateAsync()
        {
            return Task.CompletedTask;
        }
    }
}