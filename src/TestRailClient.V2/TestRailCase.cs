using System.Text.Json.Serialization;

namespace Ycode.TestRailClient.V2
{
	public class TestRailCase
    {
        [JsonPropertyName("id")]
    	public int Id { get; set; }
        [JsonPropertyName("is_completed")]
    	public bool IsCompleted { get; set; }
        [JsonPropertyName("priority_id")]
    	public TestRailPriority Priority { get; set; }
    }
}
