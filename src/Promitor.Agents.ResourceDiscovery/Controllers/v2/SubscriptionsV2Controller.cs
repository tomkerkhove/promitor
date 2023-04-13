using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Promitor.Agents.ResourceDiscovery.Graph.Exceptions;
using Promitor.Agents.ResourceDiscovery.Graph.Repositories.Interfaces;
using Promitor.Agents.ResourceDiscovery.Scheduling;
using Promitor.Core.Contracts;

namespace Promitor.Agents.ResourceDiscovery.Controllers.v2
{
    /// <summary>
    /// API endpoint to list Azure subscriptions
    /// </summary>
    [ApiController]
    [Route("api/v2/subscriptions")]
    public class SubscriptionsV2Controller : ControllerBase
    {
        private readonly JsonSerializerSettings _serializerSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionsV2Controller"/> class.
        /// </summary>
        public SubscriptionsV2Controller(IAzureResourceRepository azureResourceRepository)
        {
            Guard.NotNull(azureResourceRepository, nameof(azureResourceRepository));

            _serializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Objects
            };
            _serializerSettings.Converters.Add(new StringEnumConverter());
        }

        /// <summary>
        /// List subscriptions
        /// </summary>
        [HttpGet(Name = "SubscriptionsV2_Get")]
        [ProducesResponseType(typeof(List<AzureSubscription>), StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            var serializedResources = JsonConvert.SerializeObject(AzureSubscriptionDiscoveryBackgroundJob.CurrentSubscriptions, _serializerSettings);

            var response = Content(serializedResources, "application/json");
            response.StatusCode = (int) HttpStatusCode.OK;

            return response;
        }
    }
}
