using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Promitor.Agents.Core.Serialization
{
    /// <summary>
    /// JSON converter to deserialize <see cref="HealthReportEntry"/> <c>struct</c>.
    /// The <see cref="HealthReportEntry"/> <c>struct</c>s is not correctly deserialized since it was not especially made to be deserialized;
    /// also <c>struct</c>s are naturally not made for deserialization using Newtonsoft.Json.
    /// </summary>
    public class HealthReportEntryConverter : JsonConverter
    {
        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <summary>Reads the JSON representation of the object.</summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            
            var healthStatus = token["status"]?.ToObject<HealthStatus>() ?? HealthStatus.Degraded;
            var description = token["description"]?.ToObject<string>();
            var duration = token["duration"]?.ToObject<TimeSpan>() ?? TimeSpan.Zero;
            var exception = token["exception"]?.ToObject<Exception>();
            var data = token["data"]?.ToObject<Dictionary<string, object>>();
            var readOnlyDictionary = new ReadOnlyDictionary<string, object>(data ?? new Dictionary<string, object>());
            var tags = token["tags"]?.ToObject<string[]>();

            return new HealthReportEntry(healthStatus, description, duration, exception, readOnlyDictionary, tags);
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// 	<c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            return typeof(HealthReportEntry) == objectType;
        }
    }
}
