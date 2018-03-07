using System.Threading.Tasks;

namespace Promitor.Scraper.Validation
{
    public interface IValidation
    {
        Task ValidateAsync();
    }
}