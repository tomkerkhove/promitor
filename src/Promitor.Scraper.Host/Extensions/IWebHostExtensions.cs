using GuardNet;
using Microsoft.AspNetCore.Hosting;
using Promitor.Scraper.Host.Validation;

// ReSharper disable once CheckNamespace
namespace Promitor.Scraper.Host.Extensions
{
    public static class WebHostExtensions
    {
        public static IWebHost ValidateSetup(this IWebHost webHost)
        {
            Guard.NotNull(webHost, nameof(webHost));

            var runtimeValidator = new RuntimeValidator();
            runtimeValidator.Run();

            return webHost;
        }
    }
}