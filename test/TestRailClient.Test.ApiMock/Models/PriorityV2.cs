using System.Text.Json.Serialization;
using NJsonSchema.Annotations;

namespace Ycode.TestRailClient.Test.ApiMock.Models
{
    [JsonSchema(name: "Priority")]
    public class PriorityV2
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("short_name")]
        public string ShortName { get; set; }
        [JsonPropertyName("priority")]
        public int Priority { get; set; }
        [JsonPropertyName("is_default")]
        public bool IsDefault { get; set; }
    }
}
