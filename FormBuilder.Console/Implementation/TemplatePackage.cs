using FormBuilder.Console.Interfaces;
using Newtonsoft.Json;

namespace FormBuilder.Console.Implementation
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TemplatePackage : ITemplatePackage
    {
        [JsonProperty(Required = Required.Always)]
        public ApiPackage ApiPackage { get; set; }
    }
}