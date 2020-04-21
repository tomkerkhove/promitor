using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Promitor.Agents.ResourceDiscovery.Graph
{
    public class GraphQuery
    {
        private const string SubscriptionIdFieldName = "subscriptionId";
        private const string ResourceGroupFieldName = "resourceGroup";
        private const string RegionFieldName = "location";

        private readonly StringBuilder _queryBuilder;
        public GraphQuery(string resourceType)
        {
            _queryBuilder = new StringBuilder();
            _queryBuilder.AppendLine("Resources");
            _queryBuilder.AppendLine($"| where type == '{resourceType}'");
        }

        public static GraphQuery ForResourceType(string resourceType)
        {
            return new GraphQuery(resourceType);
        }

        public GraphQuery WithSubscriptionsWithIds(List<string> subscriptionIds)
        {
            if (subscriptionIds == null || subscriptionIds.Any() == false)
            {
                return this;
            }

            FilterByField(SubscriptionIdFieldName, subscriptionIds);

            return this;
        }

        public GraphQuery WithResourceGroupsWithName(List<string> resourceGroups)
        {
            if (resourceGroups == null || resourceGroups.Any() == false)
            {
                return this;
            }

            FilterByField(ResourceGroupFieldName, resourceGroups);

            return this;
        }

        public GraphQuery WithinRegions(List<string> regions)
        {
            if (regions == null || regions.Any() == false)
            {
                return this;
            }
            
            FilterByField(RegionFieldName, regions);

            return this;
        }

        public GraphQuery WithTags(Dictionary<string, string> tags)
        {
            if (tags == null || tags.Any() == false)
            {
                return this;
            }

            FilterByTags(tags);

            return this;
        }

        public GraphQuery Project(params string[] fields)
        {
            _queryBuilder.Append("| project ");
            for (int fieldCount = 0; fieldCount < fields.Length - 1; fieldCount++)
            {
                _queryBuilder.Append($"{fields[fieldCount]}, ");
            }

            _queryBuilder.AppendLine($"{fields.Last()}");

            return this;
        }

        public GraphQuery LimitTo(int limit)
        {
            _queryBuilder.AppendLine($"| limit {limit}");
            return this;
        }

        public string Build()
        {
            return _queryBuilder.ToString();
        }

        private void FilterByField(string fieldName, List<string> allowedValues)
        {
            _queryBuilder.Append("| where ");
            for (int counter = 0; counter < allowedValues.Count - 1; counter++)
            {
                _queryBuilder.Append($"{fieldName} == '{allowedValues[counter]}' or ");
            }

            _queryBuilder.AppendLine($"{fieldName} == '{allowedValues.Last()}'");
        }

        private void FilterByTags(Dictionary<string,string> allowedTags)
        {
            _queryBuilder.Append("| where ");
            for (int counter = 0; counter < allowedTags.Count - 1; counter++)
            {
                var dictionaryEntry = allowedTags.ElementAt(counter);
                _queryBuilder.Append($"tags['{dictionaryEntry.Key}'] == '{dictionaryEntry.Value}' or ");
            }

            var lastDictionaryEntry = allowedTags.Last();
            _queryBuilder.AppendLine($"tags['{lastDictionaryEntry.Key}'] == '{lastDictionaryEntry.Value}'");
        }
    }
}