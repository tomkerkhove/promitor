using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Promitor.Agents.Core.Controllers
{
    public class OperationsController : Controller
    {
        protected HealthCheckService HealthCheckService { get; }

        public OperationsController(HealthCheckService healthCheckService)
        {
            Guard.NotNull(healthCheckService, nameof(healthCheckService));

            HealthCheckService = healthCheckService;
        }

        /// <summary>
        ///     Get health of the API
        /// </summary>
        /// <remarks>Provides an indication about the health of the API</remarks>
        /// <param name="includeDependencies">
        ///     Indication whether or not dependencies integration should be verified.
        ///     Do note that this will contact all dependencies which can have performance impact or cascading failures when
        ///     consumed very often.
        /// </param>
        /// <response code="200">API is healthy</response>
        /// <response code="503">API is unhealthy or in degraded state</response>
        protected async Task<IActionResult> GetHealthAsync(bool includeDependencies)
        {
            if (includeDependencies == false)
            {
                // We are creating a basic report to align with the contract that we return.
                var healthReport = new HealthReport(new Dictionary<string, HealthReportEntry>(), TimeSpan.Zero);
                return Ok(healthReport);
            }

            var report = await HealthCheckService.CheckHealthAsync();

            return report.Status == HealthStatus.Healthy ? Ok(report) : StatusCode((int)HttpStatusCode.ServiceUnavailable, report);
        }
    }
}
