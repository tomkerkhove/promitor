using Swashbuckle.AspNetCore.Annotations;

namespace Promitor.Agents.Core.Contracts
{
    public class SystemInfo
    {
        [SwaggerSchema("Version of the agent")]
        public string Version { get; set; }
    }
}