using GuardNet;
using Microsoft.AspNetCore.Builder;

// ReSharper disable once CheckNamespace
namespace Swashbuckle.AspNetCore.SwaggerUI
{
    // ReSharper disable once InconsistentNaming
    internal static class SwaggerUIOptionsExtensions
    {
        /// <summary>
        /// Configure the default options for our Swagger UI portal
        /// </summary>
        /// <param name="swaggerUiOptions">Swagger UI portal options</param>
        internal static SwaggerUIOptions ConfigureDefaultOptions(this SwaggerUIOptions swaggerUiOptions)
        {
            Guard.NotNull(swaggerUiOptions, nameof(swaggerUiOptions));

            swaggerUiOptions.DisplayOperationId();
            swaggerUiOptions.EnableDeepLinking();
            swaggerUiOptions.DocumentTitle = "Promitor API";
            swaggerUiOptions.DocExpansion(DocExpansion.List);
            swaggerUiOptions.DisplayRequestDuration();
            swaggerUiOptions.EnableFilter();

            return swaggerUiOptions;
        }
    }
}
