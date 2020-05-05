using Swashbuckle.AspNetCore.SwaggerUI;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder
{
    // ReSharper disable once InconsistentNaming
    public static class IApplicationBuilderExtensions
    {
        /// <summary>
        ///     Add support for Open API with API explorer
        /// </summary>
        /// <param name="app">Application Builder</param>
        /// <param name="apiName">Name of API</param>
        public static IApplicationBuilder ExposeOpenApiUi(this IApplicationBuilder app, string apiName)
        {
            // New Swagger UI
            app.UseSwagger(setupAction => setupAction.RouteTemplate = "api/{documentName}/docs.json");
            app.UseSwaggerUI(swaggerUiOptions =>
            {
                swaggerUiOptions.ConfigureDefaultOptions();
                swaggerUiOptions.SwaggerEndpoint("/api/v1/docs.json", apiName);
                swaggerUiOptions.RoutePrefix = "api/docs";
            });

            return app;
        }
    }
}
