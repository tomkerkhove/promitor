using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GuardNet;

namespace Promitor.Agents.ResourceDiscovery.Graph.Query
{
    public class GraphQueryBuilder
    {
        private const string SubscriptionIdFieldName = "subscriptionId";
        private const string ResourceGroupFieldName = "resourceGroup";
        private const string RegionFieldName = "location";

        private readonly StringBuilder _queryBuilder;
        public GraphQueryBuilder(string resourceType)
            :this(new []{resourceType})
        {    
        }

        public GraphQueryBuilder(string[] resourceTypes)
        {
            Guard.NotNull(resourceTypes,nameof(resourceTypes));
            Guard.For<ArgumentException>(()=> resourceTypes.Length == 0, nameof(resourceTypes));

            _queryBuilder = new StringBuilder();
            _queryBuilder.AppendLine("Resources");
            _queryBuilder.AppendLine($"| where type =~ '{resourceTypes.First()}'");

            foreach (var resourceType in resourceTypes.Skip(1))
            {
                _queryBuilder.AppendLine($" or type =~ '{resourceType}'");
            }
        }

        public static GraphQueryBuilder ForResourceType(string resourceType)
        {
            return new GraphQueryBuilder(resourceType);
        }

        public static GraphQueryBuilder ForResourceType(params string[] resourceTypes)
        {
            return new GraphQueryBuilder(resourceTypes);
        }

        public GraphQueryBuilder Where(string field, Operator @operator, string value)
        {
            var queryOperator = GetQueryOperator(@operator);
            _queryBuilder.AppendLine($"| where {field} {queryOperator} '{value}'");

            return this;
        }

        private string GetQueryOperator(Operator operatorToUse)
        {
            switch (operatorToUse)
            {
                case Operator.Equals:
                    return "==";
                case Operator.DoesNotEquals:
                    return "!=";
                case Operator.Contains:
                    return "contains";
                case Operator.DoesNotContain:
                    return "!contains";
                default:
                    throw new ArgumentOutOfRangeException(nameof(operatorToUse), operatorToUse, null);
            }
        }

        public GraphQueryBuilder WithSubscriptionsWithIds(List<string> subscriptionIds)
        {
            if (subscriptionIds == null || subscriptionIds.Any() == false)
            {
                return this;
            }

            FilterByField(SubscriptionIdFieldName, subscriptionIds);

            return this;
        }

        public GraphQueryBuilder WithResourceGroupsWithName(List<string> resourceGroups)
        {
            if (resourceGroups == null || resourceGroups.Any() == false)
            {
                return this;
            }

            FilterByField(ResourceGroupFieldName, resourceGroups);

            return this;
        }

        public GraphQueryBuilder WithinRegions(List<string> regions)
        {
            if (regions == null || regions.Any() == false)
            {
                return this;
            }
            
            FilterByField(RegionFieldName, regions);

            return this;
        }

        public GraphQueryBuilder WithTags(Dictionary<string, string> tags)
        {
            if (tags == null || tags.Any() == false)
            {
                return this;
            }

            FilterByTags(tags);

            return this;
        }

        public GraphQueryBuilder Project(params string[] fields)
        {
            _queryBuilder.Append("| project ");
            for (int fieldCount = 0; fieldCount < fields.Length - 1; fieldCount++)
            {
                _queryBuilder.Append($"{fields[fieldCount]}, ");
            }

            _queryBuilder.AppendLine($"{fields.Last()}");

            return this;
        }

        public GraphQueryBuilder LimitTo(int limit)
        {
            _queryBuilder.AppendLine($"| limit {limit}");
            return this;
        }

        public string Build()
        {
            return _queryBuilder.ToString().Trim();
        }

        private void FilterByField(string fieldName, List<string> allowedValues)
        {
            _queryBuilder.Append("| where ");
            for (int counter = 0; counter < allowedValues.Count - 1; counter++)
            {
                _queryBuilder.Append($"{fieldName} =~ '{allowedValues[counter]}' or ");
            }

            _queryBuilder.AppendLine($"{fieldName} =~ '{allowedValues.Last()}'");
        }

        private void FilterByTags(Dictionary<string, string> allowedTags)
        {
            _queryBuilder.Append("| where ");
            for (int counter = 0; counter < allowedTags.Count - 1; counter++)
            {
                var dictionaryEntry = allowedTags.ElementAt(counter);
                _queryBuilder.Append($"tags['{dictionaryEntry.Key}'] matches regex @'^(?i){dictionaryEntry.Value}$' or ");
            }

            var lastDictionaryEntry = allowedTags.Last();
            _queryBuilder.AppendLine($"tags['{lastDictionaryEntry.Key}'] matches regex @'^(?i){lastDictionaryEntry.Value}$'");
        }
    }
}
