using System;
using System.Text.Json.Serialization;
using NJsonSchema.Annotations;

namespace Ycode.TestRailClient.Test.ApiMock.Models
{
    [JsonSchema(name: "RunInfo")]
    public class RunInfoV2
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

    [JsonSchema(name: "Run")]
    public class RunV2
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("project_id")]
        public int ProjectId { get; set; }
        [JsonPropertyName("suite_id")]
        public int SuiteId { get; set; }
        [JsonPropertyName("plan_id")]
        public int PlanId { get; set; }
        [JsonPropertyName("milestone_id")]
        public int MilestoneId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("refs")]
        public string Refs { get; set; }
        [JsonPropertyName("blocked_count")]
        public int BlockedCount { get; set; }
        [JsonPropertyName("custom_status_count")]
        public int CustomStatusCount { get; set; }
        [JsonPropertyName("failed_count")]
        public int FailedCount { get; set; }
        [JsonPropertyName("passed_count")]
        public int PassedCount { get; set; }
        [JsonPropertyName("retest_count")]
        public int RetestCount { get; set; }
        [JsonPropertyName("untested_count")]
        public int UntestedCount { get; set; }
    }
}
