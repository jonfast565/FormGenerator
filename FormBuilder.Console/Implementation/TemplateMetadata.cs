using FormBuilder.Console.Enums;
using FormBuilder.Console.Interfaces;
using Newtonsoft.Json;

namespace FormBuilder.Console.Implementation
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TemplateMetadata : ITemplateMetadata
    {
        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string OutputFolder { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Path { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Filename { get; set; }

        [JsonProperty(Required = Required.Always)]
        public TemplateScope Scope { get; set; }
    }
}