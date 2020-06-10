using System.Text.Json.Serialization;
using NJsonSchema.Annotations;

namespace Ycode.TestRailClient.Test.ApiMock.Models
{
    [JsonSchema(name: "Case")]
    public class CaseV2
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("suite_id")]
        public int SuiteId { get; set; }
        [JsonPropertyName("section_id")]
        public int SectionId { get; set; }
        [JsonPropertyName("template_id")]
        public int TemplateId { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("priority_id")]
        public int PriorityId { get; set; }
        [JsonPropertyName("type_id")]
        public int TypeId { get; set; }
        [JsonPropertyName("refs")]
        public string Refs { get; set; }
    }
}
