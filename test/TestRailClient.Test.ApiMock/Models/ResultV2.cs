using System.Text.Json.Serialization;
using NJsonSchema.Annotations;

namespace Ycode.TestRailClient.Test.ApiMock.Models
{
    [JsonSchema(name: "ResultInfo")]
	public class ResultInfoV2
    {
        [JsonPropertyName("status_id")]
    	public int Status { get; set; }
        [JsonPropertyName("comment")]
    	public string Comment { get; set; }
        [JsonPropertyName("version")]
    	public string Version { get; set; }
        [JsonPropertyName("defects")]
    	public string Defects { get; set; }
        [JsonPropertyName("elapsed")]
    	public string Elapsed { get; set; }
        [JsonPropertyName("assignedto_id")]
    	public int AssignedToId { get; set; }
    }

    [JsonSchema(name: "Result")]
	public class ResultV2
    {
        [JsonPropertyName("id")]
    	public int Id { get; set; }
        [JsonPropertyName("test_id")]
    	public int TestId { get; set; }
        [JsonPropertyName("status_id")]
    	public int Status { get; set; }
        [JsonPropertyName("comment")]
    	public string Comment { get; set; }
        [JsonPropertyName("version")]
    	public string Version { get; set; }
        [JsonPropertyName("defects")]
    	public string Defects { get; set; }
        [JsonPropertyName("elapsed")]
    	public string Elapsed { get; set; }
        [JsonPropertyName("assignedto_id")]
    	public int AssignedToId { get; set; }
    }
}
