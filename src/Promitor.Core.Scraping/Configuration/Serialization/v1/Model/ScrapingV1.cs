namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model
{
    public class ScrapingV1
    {
        public string Schedule { get; set; }

        public Configuration.Model.Scraping Build()
        {
            return new Configuration.Model.Scraping
            {
                Schedule = Schedule
            };
        }
    }
}
