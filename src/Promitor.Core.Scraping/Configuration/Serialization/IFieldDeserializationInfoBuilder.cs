namespace Promitor.Core.Scraping.Configuration.Serialization
{
    /// <summary>
    /// An object that can build <see cref="FieldDeserializationInfo" /> objects.
    /// </summary>
    public interface IFieldDeserializationInfoBuilder
    {
        /// <summary>
        /// Builds the deserialization info object.
        /// </summary>
        FieldDeserializationInfo Build();
    }
}