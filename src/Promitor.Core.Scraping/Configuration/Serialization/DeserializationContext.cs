using System.Collections.Generic;
using System.Linq;
using Fastenshtein;

namespace Promitor.Core.Scraping.Configuration.Serialization
{
    /// <summary>
    /// Contains the information needed to deserialize a node.
    /// </summary>
    public class DeserializationContext<TObject>
    {
        /// <summary>
        /// The max edit distance for us to consider a field as a suggestion
        /// between a field name entered by a user, and the field names that
        /// have been configured for the object.
        /// </summary>
        private const int MaxSuggestionDistance = 3;

        private readonly ISet<string> _ignoredFields;
        private readonly Dictionary<string, FieldDeserializationContext<TObject>> _fields;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeserializationContext{TObject}"/> type.
        /// </summary>
        /// <param name="ignoredFields">The fields that should be ignored.</param>
        /// <param name="fields">The fields that can be deserialized.</param>
        public DeserializationContext(ISet<string> ignoredFields, IReadOnlyCollection<FieldDeserializationInfo> fields)
        {
            _ignoredFields = ignoredFields;
            _fields = fields.ToDictionary(
                fieldInfo => fieldInfo.YamlFieldName, fieldInfo => new FieldDeserializationContext<TObject>(fieldInfo));
        }

        /// <summary>
        /// The fields that have not been set during deserialization.
        /// </summary>
        public IReadOnlyCollection<FieldDeserializationContext<TObject>> UnsetFields
        {
            get
            {
                return _fields
                    .Where(f => !f.Value.HasBeenSet)
                    .Select(f => f.Value)
                    .ToList();
            }
        }

        /// <summary>
        /// Returns whether the specified field is ignored.
        /// </summary>
        /// <param name="fieldName">The field name.</param>
        /// <returns>true if the field is ignored, false otherwise.</returns>
        public bool IsIgnored(string fieldName)
        {
            return _ignoredFields.Contains(fieldName);
        }

        /// <summary>
        /// Tries to find the specified field.
        /// </summary>
        /// <param name="fieldName">The field name.</param>
        /// <param name="field">Set to the field if found, otherwise null.</param>
        /// <returns>true if the field was found, false otherwise.</returns>
        public bool TryGetField(string fieldName, out FieldDeserializationContext<TObject> field)
        {
            return _fields.TryGetValue(fieldName, out field);
        }

        /// <summary>
        /// Returns a list of fields that are close enough to <paramref ref="fieldName" />
        /// for us to suggest them as alternatives.
        /// </summary>
        /// <param name="fieldName">
        /// The name of a field that doesn't exist that we want to find suggestions for.
        /// </param>
        public IReadOnlyCollection<string> GetSuggestions(string fieldName)
        {
            return _fields
                .Select(field => field.Key)
                .Union(_ignoredFields)
                .Where(suggestion => Levenshtein.Distance(suggestion, fieldName) < MaxSuggestionDistance)
                .ToList();
        }
    }
}
