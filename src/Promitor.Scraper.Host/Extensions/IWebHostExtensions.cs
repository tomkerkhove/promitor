using GuardNet;
using Promitor.Scraper.Host.Validation;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Hosting
{
    public static class IWebHostExtensions
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