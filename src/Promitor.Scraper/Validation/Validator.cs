using System.Collections.Generic;
using Promitor.Scraper.Validation.Interfaces;

namespace Promitor.Scraper.Validation
{
    public static class Validator
    {
        public static void Run(IEnumerable<IValidationStep> validationSteps)
        {
            foreach (var validationStep in validationSteps)
            {
                validationStep.Validate();
            }
        }
    }
}