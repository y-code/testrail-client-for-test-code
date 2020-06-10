using System.Text.Json.Serialization;

namespace Ycode.TestRailClient.V2
{
	public class TestRailRunInfo
    {
        [JsonPropertyName("project_id")]
    	public int ProjectId { get; set; }
        [JsonPropertyName("suite_id")]
    	public int SuiteId { get; set; }
        [JsonPropertyName("milestone_id")]
    	public int MilestoneId { get; set; }
        [JsonPropertyName("assignedto_id")]
    	public int AssignedtoId { get; set; }
        [JsonPropertyName("name")]
    	public string Name { get; set; }
        [JsonPropertyName("description")]
    	public string Description { get; set; }
        [JsonPropertyName("include_all")]
    	public bool IncludeAll { get; set; }
        [JsonPropertyName("case_ids")]
    	public int[] CaseIds { get; set; }
        [JsonPropertyName("refs")]
    	public string Refs { get; set; }
    }

	public class TestRailRun
    {
        [JsonPropertyName("project_id")]
    	public int ProjectId { get; set; }
        [JsonPropertyName("suite_id")]
    	public int SuiteId { get; set; }
        [JsonPropertyName("milestone_id")]
    	public int MilestoneId { get; set; }
        [JsonPropertyName("assignedto_id")]
    	public int AssignedtoId { get; set; }
        [JsonPropertyName("name")]
    	public string Name { get; set; }
        [JsonPropertyName("description")]
    	public string Description { get; set; }
        [JsonPropertyName("include_all")]
    	public bool IncludeAll { get; set; }
        [JsonPropertyName("refs")]
    	public string Refs { get; set; }
        [JsonPropertyName("id")]
    	public int Id { get; set; }
        [JsonPropertyName("is_completed")]
    	public bool IsCompleted { get; set; }
    }
}
