using GuardNet;

namespace Promitor.Core.Metrics;

public class MeasuredMetricDimension
{
    /// <summary>
    ///     Name of dimension
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Value of dimension
    /// </summary>
    public string Value { get; }

    public MeasuredMetricDimension(string dimensionName, string dimensionValue)
    {
        Guard.NotNullOrWhitespace(dimensionName, nameof(dimensionName));
        Guard.NotNullOrWhitespace(dimensionValue, nameof(dimensionValue));

        Value = dimensionValue;
        Name = dimensionName;
    }
}