using System;
using System.Collections.Generic;
using Promitor.Agents.ResourceDiscovery.Graph.Query;
using Xunit;

namespace Promitor.Tests.Unit.Builders.Queries
{
    public class GraphQueryBuilderTests
    {
        private const string ResourceType = "resource type";

        [Fact]
        public void ForResourceType_SingleResourceType_ReturnsValidGraphQueryBuilder()
        {
            // Arrange
            string expectedQuery =
                $"Resources{Environment.NewLine}" +
                "| where type =~ 'resource type'";

            // Act
            GraphQueryBuilder graphQueryBuilder = GraphQueryBuilder.ForResourceType(ResourceType);

            // Assert
            Assert.Equal(expectedQuery, graphQueryBuilder.Build());
        }

        [Fact]
        public void ForResourceType_TwoResourceTypes_ReturnsValidGraphQueryBuilder()
        {
            // Arrange
            const string resourceType1 = "resource type 1";
            const string resourceType2 = "resource type 2";
            string expectedQuery =
                $"Resources{Environment.NewLine}" +
                $"| where type =~ 'resource type 1'{Environment.NewLine}" +
                " or type =~ 'resource type 2'";

            // Act
            GraphQueryBuilder graphQueryBuilder = GraphQueryBuilder.ForResourceType(resourceType1, resourceType2);

            // Assert
            Assert.Equal(expectedQuery, graphQueryBuilder.Build());
        }

        [Theory]
        [InlineData(Operator.Contains, "contains")]
        [InlineData(Operator.DoesNotContain, "!contains")]
        [InlineData(Operator.DoesNotEquals, "!=")]
        [InlineData(Operator.Equals, "==")]
        public void Where_AppendCorrectQuery(Operator @operator, string queryOperatopr)
        {
            // Arrange
            string expectedQuery =
                $"Resources{Environment.NewLine}" +
                $"| where type =~ 'resource type'{Environment.NewLine}" +
                "| where field " + queryOperatopr + " 'value'";

            GraphQueryBuilder graphQueryBuilder = GraphQueryBuilder.ForResourceType(ResourceType);

            // Act
            graphQueryBuilder.Where("field", @operator, "value");

            // Assert
            Assert.Equal(expectedQuery, graphQueryBuilder.Build());
        }

        [Fact]
        public void WithSubscriptionsWithIds_AppendCorrectQuery()
        {
            // Arrange
            string expectedQuery =
                $"Resources{Environment.NewLine}" +
                $"| where type =~ 'resource type'{Environment.NewLine}" +
                "| where subscriptionId =~ 'subscription Id 1' or subscriptionId =~ 'subscription Id 2' or subscriptionId =~ 'subscription Id 3'";

            GraphQueryBuilder graphQueryBuilder = GraphQueryBuilder.ForResourceType(ResourceType);

            List<string> subscriptionIds = new List<string>
            {
                "subscription Id 1",
                "subscription Id 2",
                "subscription Id 3"
            };

            // Act
            graphQueryBuilder.WithSubscriptionsWithIds(subscriptionIds);

            // Assert
            Assert.Equal(expectedQuery, graphQueryBuilder.Build());
        }

        [Fact]
        public void WithResourceGroupsWithName_AppendCorrectQuery()
        {
            // Arrange
            string expectedQuery =
                $"Resources{Environment.NewLine}" +
                $"| where type =~ 'resource type'{Environment.NewLine}" +
                "| where resourceGroup =~ 'resourceGroup 1' or resourceGroup =~ 'resourceGroup 2' or resourceGroup =~ 'resourceGroup 3'";

            GraphQueryBuilder graphQueryBuilder = GraphQueryBuilder.ForResourceType(ResourceType);

            List<string> resourceGroups = new List<string>
            {
                "resourceGroup 1",
                "resourceGroup 2",
                "resourceGroup 3"
            };

            // Act
            graphQueryBuilder.WithResourceGroupsWithName(resourceGroups);

            // Assert
            Assert.Equal(expectedQuery, graphQueryBuilder.Build());
        }

        [Fact]
        public void WithinRegions_AppendCorrectQuery()
        {
            // Arrange
            string expectedQuery =
                $"Resources{Environment.NewLine}" +
                $"| where type =~ 'resource type'{Environment.NewLine}" +
                "| where location =~ 'region 1' or location =~ 'region 2' or location =~ 'region 3'";

            GraphQueryBuilder graphQueryBuilder = GraphQueryBuilder.ForResourceType(ResourceType);

            List<string> regions = new List<string>
            {
                "region 1",
                "region 2",
                "region 3"
            };

            // Act
            graphQueryBuilder.WithinRegions(regions);

            // Assert
            Assert.Equal(expectedQuery, graphQueryBuilder.Build());
        }

        [Fact]
        public void WithinTags_AppendCorrectQuery()
        {
            // Arrange
            string expectedQuery =
                $"Resources{Environment.NewLine}" +
                $"| where type =~ 'resource type'{Environment.NewLine}" +
                "| where tags['tag key 1'] matches regex @'^(?i)tag value 1$' or tags['tag key 2'] matches regex @'^(?i)tag value 2$' or tags['tag key 3'] matches regex @'^(?i)tag value 3$'";

            GraphQueryBuilder graphQueryBuilder = GraphQueryBuilder.ForResourceType(ResourceType);

            Dictionary<string, string> tags = new Dictionary<string, string>
            {
                ["tag key 1"] = "tag value 1",
                ["tag key 2"] = "tag value 2",
                ["tag key 3"] = "tag value 3",
            };

            // Act
            graphQueryBuilder.WithTags(tags);

            // Assert
            Assert.Equal(expectedQuery, graphQueryBuilder.Build());
        }

        [Fact]
        public void Project_SingleField_ReturnsValidGraphQueryBuilder()
        {
            // Arrange
            string expectedQuery =
                $"Resources{Environment.NewLine}" +
                $"| where type =~ 'resource type'{Environment.NewLine}" +
                "| project field";

            GraphQueryBuilder graphQueryBuilder = GraphQueryBuilder.ForResourceType(ResourceType);

            // Act
            graphQueryBuilder.Project("field");

            // Assert
            Assert.Equal(expectedQuery, graphQueryBuilder.Build());
        }

        [Fact]
        public void Project_TwoFields_ReturnsValidGraphQueryBuilder()
        {
            // Arrange
            const string field1 = "field 1";
            const string field2 = "field 2";

            string expectedQuery =
                $"Resources{Environment.NewLine}" +
                $"| where type =~ 'resource type'{Environment.NewLine}" +
                "| project field 1, field 2";

            GraphQueryBuilder graphQueryBuilder = GraphQueryBuilder.ForResourceType(ResourceType);

            // Act
            graphQueryBuilder.Project(field1, field2);

            // Assert
            Assert.Equal(expectedQuery, graphQueryBuilder.Build());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(17)]
        [InlineData(58)]
        public void LimitTo_AppendCorrectQuery(int limit)
        {
            // Arrange
            string expectedQuery =
                $"Resources{Environment.NewLine}" +
                $"| where type =~ 'resource type'{Environment.NewLine}" +
                "| limit " + limit;

            GraphQueryBuilder graphQueryBuilder = GraphQueryBuilder.ForResourceType(ResourceType);

            // Act
            graphQueryBuilder.LimitTo(limit);

            // Assert
            Assert.Equal(expectedQuery, graphQueryBuilder.Build());
        }
    }
}
