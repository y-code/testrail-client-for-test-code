using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ycode.TestRailClient.V2
{
	public class TestRailPriority
    {
    	public static readonly TestRailPriority Dummy = new TestRailPriority
        {
        	Id = 0
        };

        [JsonPropertyName("id")]
    	public int Id { get; set; }
        [JsonPropertyName("priority")]
    	public int Priority { get; set; }
        [JsonPropertyName("name")]
    	public string Name { get; set; }
        [JsonPropertyName("short_name")]
    	public string ShortName { get; set; }
        [JsonPropertyName("is_default")]
    	public bool IsDefault { get; set; }
    }

	public class TestRailPriorityConverter : JsonConverter<TestRailPriority>
    {
    	IReadOnlyDictionary<int, TestRailPriority> _priorities;

    	public TestRailPriorityConverter(IReadOnlyDictionary<int, TestRailPriority> priorities)
        {
        	_priorities = priorities;
        }

    	public override TestRailPriority Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
        	if (reader.TryGetInt32(out var priorityId) && _priorities.TryGetValue(priorityId, out var priority))
            {
            	return priority;
            }
        	return TestRailPriority.Dummy;
        }

    	public override void Write(Utf8JsonWriter writer, TestRailPriority value, JsonSerializerOptions options)
        {
        	writer.WriteNumberValue(value.Id);
        }
    }
}
