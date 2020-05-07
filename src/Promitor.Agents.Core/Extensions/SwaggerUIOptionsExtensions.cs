using GuardNet;
using Microsoft.AspNetCore.Builder;

// ReSharper disable once CheckNamespace
namespace Swashbuckle.AspNetCore.SwaggerUI
{
    // ReSharper disable once InconsistentNaming
    public static class SwaggerUIOptionsExtensions
    {
        /// <summary>
        /// Configure the default options for our Swagger UI portal
        /// </summary>
        /// <param name="swaggerUiOptions">Swagger UI portal options</param>
        /// <param name="documentTitle">Title of the UI</param>
        public static SwaggerUIOptions ConfigureDefaultOptions(this SwaggerUIOptions swaggerUiOptions, string documentTitle)
        {
            Guard.NotNull(swaggerUiOptions, nameof(swaggerUiOptions));

            swaggerUiOptions.DisplayOperationId();
            swaggerUiOptions.EnableDeepLinking();
            swaggerUiOptions.DocumentTitle = documentTitle;
            swaggerUiOptions.DocExpansion(DocExpansion.List);
            swaggerUiOptions.DisplayRequestDuration();
            swaggerUiOptions.EnableFilter();

            return swaggerUiOptions;
        }
    }
}
