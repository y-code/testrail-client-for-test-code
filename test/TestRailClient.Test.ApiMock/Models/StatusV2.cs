using System;
using System.Text.Json.Serialization;
using NJsonSchema.Annotations;

namespace Ycode.TestRailClient.Test.ApiMock.Models
{
    [JsonSchema(name: "Status")]
    public class StatusV2
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("label")]
        public string Label { get; set; }
        [JsonPropertyName("is_final")]
        public bool IsFinal { get; set; }
        [JsonPropertyName("is_system")]
        public bool IsSystem { get; set; }
        [JsonPropertyName("is_untested")]
        public bool IsUntested { get; set; }
    }
}
