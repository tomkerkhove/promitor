using System.Linq;
using System.Net.Http.Headers;
using GuardNet;

namespace Promitor.Tests.Integration.Extensions
{
    public static class HttpResponseHeadersExtensions
    {
        /// <summary>
        ///     Gets the first or default value for a given HTTP header
        /// </summary>
        public static string GetFirstOrDefaultHeaderValue(this HttpResponseHeaders headers, string headerName)
        {
            Guard.NotNull(headers, nameof(headers));
            Guard.NotNullOrWhitespace(headerName, nameof(headerName));

            return headers.GetValues(headerName).FirstOrDefault();
        }
    }
}