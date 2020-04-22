using GuardNet;

namespace Promitor.Agents.ResourceDiscovery.Health
{
    public class HealthProbeResult
    {
        public string Subscription { get; }
        public bool IsSuccessful { get; }
        public string Description { get; }

        public HealthProbeResult(string subscription, bool isSuccessful, string description)
        {
            Guard.NotNullOrWhitespace(subscription, nameof(subscription));
            Guard.NotNullOrWhitespace(description, nameof(description));

            Subscription = subscription;
            IsSuccessful = isSuccessful;
            Description = description;
        }
    }
}