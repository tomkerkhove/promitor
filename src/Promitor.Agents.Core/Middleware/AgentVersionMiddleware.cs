
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Promitor.Core;

namespace Promitor.Agents.Core.Middleware
{
    public class AgentVersionMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AgentVersionMiddleware" /> class which automatically adds the version of the agent to all HTTP responses
        /// </summary>
        /// <param name="next">The next <see cref="RequestDelegate" /> in the ASP.NET Core request pipeline.</param>
        /// <exception cref="System.ArgumentNullException">When the <paramref name="next" /> is <c>null</c>.</exception>
        public AgentVersionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        ///     Invoke the middleware to automatically add version to HTTP responses.
        /// </summary>
        /// <param name="httpContext">The context for the current HTTP request.</param>
        public async Task Invoke(HttpContext httpContext)
        {
            var version = Version.Get();

            httpContext.Response.OnStarting(state =>
            {
                var context = (HttpContext)state;
                context.Response.Headers.TryAdd(HttpHeaders.AgentVersion, version);

                return Task.CompletedTask;
            }, httpContext);

            await _next(httpContext);
        }
    }
}
