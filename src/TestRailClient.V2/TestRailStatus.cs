using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ycode.TestRailClient.V2
{
	public class TestRailStatus
    {
    	public static readonly TestRailStatus Dummy = new TestRailStatus
        {
        	Id = 0
        };

        [JsonPropertyName("id")]
    	public int Id { get; set; }
        [JsonPropertyName("label")]
    	public string Label { get; set; }
        [JsonPropertyName("name")]
    	public string Name { get; set; }
    }

	public class TestRailStatusConverter : JsonConverter<TestRailStatus>
    {
    	IReadOnlyDictionary<int, TestRailStatus> _statuses;

    	public TestRailStatusConverter(IReadOnlyDictionary<int, TestRailStatus> statuses)
        {
        	_statuses = statuses;
        }

    	public override TestRailStatus Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
        	if (reader.TryGetInt32(out var statusId) && _statuses.TryGetValue(statusId, out var status))
            {
            	return status;
            }
        	return TestRailStatus.Dummy;
        }

    	public override void Write(Utf8JsonWriter writer, TestRailStatus value, JsonSerializerOptions options)
        {
        	writer.WriteNumberValue(value?.Id ?? TestRailStatus.Dummy.Id);
        }
    }
}
