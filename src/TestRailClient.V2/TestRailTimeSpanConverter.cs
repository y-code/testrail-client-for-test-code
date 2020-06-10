using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ycode.TestRailClient.V2
{
	public class TestRailTimeSpanConverter : JsonConverter<TimeSpan>
    {
    	public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
        	var value = reader.GetString();
        	if (value == null)
            {
            	return TimeSpan.Zero;
            }
        	if (TimeSpan.TryParseExact(reader.GetString(), new[] { "h'h 'm'm 's's'", "m'm 's's'", "s's'" }, CultureInfo.InvariantCulture, TimeSpanStyles.None, out var timeSpan))
            {
            	return timeSpan;
            }
        	return TimeSpan.Zero;
        }

    	public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
        	if (value.TotalSeconds < 1)
            {
            	writer.WriteNullValue();
            }
        	else
            {
            	writer.WriteStringValue(value.ToString("h'h 'm'm 's's'"));
            }
        }
    }
}
