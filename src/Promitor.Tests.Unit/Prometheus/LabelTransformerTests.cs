using System.Collections.Generic;
using System.ComponentModel;
using Promitor.Integrations.Sinks.Prometheus.Configuration;
using Promitor.Integrations.Sinks.Prometheus.Labels;
using Xunit;

namespace Promitor.Tests.Unit.Prometheus
{
    [Category("Unit")]
    public class LabelTransformerTests : UnitTest
    {
        [Fact]
        public void TransformLabels_NoTransformationWithDifferentCases_NoTransformationWasApplied()
        {
            // Arrange
            var inputLabels = new Dictionary<string, string>
            {
                {BogusGenerator.Name.FirstName(), BogusGenerator.Name.FirstName()},
                {BogusGenerator.Name.FirstName(), BogusGenerator.Name.FirstName()},
                {BogusGenerator.Name.FirstName(), BogusGenerator.Name.FirstName()},
                {BogusGenerator.Name.FirstName(), BogusGenerator.Name.FirstName()},
                {BogusGenerator.Name.FirstName(), BogusGenerator.Name.FirstName()},
            };

            // Act
            var transformedLabels = LabelTransformer.TransformLabels(LabelTransformation.None, inputLabels);

            // Assert
            Assert.Equal(inputLabels.Count, transformedLabels.Count);
            foreach (var inputLabel in inputLabels)
            {
                Assert.True(transformedLabels.ContainsKey(inputLabel.Key));
                Assert.Equal(inputLabel.Value, transformedLabels[inputLabel.Key]);
            }
        }

        [Fact]
        public void TransformLabels_TransformationToLowercase_LabelValuesTransformedToLowercase()
        {
            // Arrange
            var inputLabels = new Dictionary<string, string>
            {
                {BogusGenerator.Name.FirstName(), BogusGenerator.Name.FirstName().ToUpper()},
                {BogusGenerator.Name.FirstName(), BogusGenerator.Name.FirstName()},
                {BogusGenerator.Name.FirstName(), BogusGenerator.Name.FirstName().ToUpperInvariant()}
            };

            // Act
            var transformedLabels = LabelTransformer.TransformLabels(LabelTransformation.Lowercase, inputLabels);

            // Assert
            Assert.Equal(inputLabels.Count, transformedLabels.Count);
            foreach (var inputLabel in inputLabels)
            {
                Assert.True(transformedLabels.ContainsKey(inputLabel.Key));
                Assert.Equal(inputLabel.Value.ToLower(), transformedLabels[inputLabel.Key]);
            }
        }
    }
}