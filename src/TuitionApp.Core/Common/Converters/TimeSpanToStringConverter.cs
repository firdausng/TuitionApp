using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TuitionApp.Core.Common.Converters
{
    // ref - https://stackoverflow.com/questions/58283761/net-core-3-0-timespan-deserialization-error
    public class TimeSpanToStringConverter : JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return TimeSpan.Parse(value, CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(null, CultureInfo.InvariantCulture));
        }
    }
}
