using System.ComponentModel;
using Promitor.Core.Extensions;
using Promitor.Core.Metrics;
using Xunit;

namespace Promitor.Tests.Unit.Core.Extensions
{
    [Category("Unit")]
    public class MeasureMetricExtensionsTests
    {
        [Fact]
        public void AssociateWithResourceId()
        {
            var measuredMetricUnassociated = MeasuredMetric.CreateWithoutDimensions(1);
            var resourceId = "/subscriptions/abc/providers/def/test";
            var measuredMetricAssociated = measuredMetricUnassociated.WithResourceIdAssociation(resourceId);
            Assert.Equal(resourceId, measuredMetricAssociated.ResourceId);
        }
    }
}