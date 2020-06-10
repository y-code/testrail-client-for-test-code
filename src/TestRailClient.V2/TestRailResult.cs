using System;
using System.Text.Json.Serialization;

namespace Ycode.TestRailClient.V2
{
	public class TestRailResultInfo
    {
        [JsonPropertyName("status_id")]
    	public TestRailStatus Status { get; set; }
        [JsonPropertyName("comment")]
    	public string Comment { get; set; }
        [JsonPropertyName("version")]
    	public string Version { get; set; }
        [JsonPropertyName("defects")]
    	public string Defects { get; set; }
        [JsonPropertyName("elapsed")]
        [JsonConverter(typeof(TestRailTimeSpanConverter))]
    	public TimeSpan Elapsed { get; set; }
        [JsonPropertyName("assignedto_id")]
    	public int AssignedToId { get; set; }
    }

	public class TestRailResult
    {
        [JsonPropertyName("id")]
    	public int Id { get; set; }
        [JsonPropertyName("test_id")]
    	public int TestId { get; set; }
        [JsonPropertyName("status_id")]
    	public TestRailStatus Status { get; set; }
        [JsonPropertyName("comment")]
    	public string Comment { get; set; }
        [JsonPropertyName("version")]
    	public string Version { get; set; }
        [JsonPropertyName("defects")]
    	public string Defects { get; set; }
        [JsonPropertyName("elapsed")]
        [JsonConverter(typeof(TestRailTimeSpanConverter))]
    	public TimeSpan Elapsed { get; set; }
        [JsonPropertyName("assignedto_id")]
    	public int AssignedToId { get; set; }
    }
}
