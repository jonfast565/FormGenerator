using System.Collections.Generic;
using FormBuilder.Console.Interfaces;
using Newtonsoft.Json;
using System;

namespace FormBuilder.Console.Implementation
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ApiPackage : IApiPackage
    {
        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string OutputFolder { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string ApiRootUrl { get; set; }

        [JsonProperty(Required = Required.Always)]
        public IList<ApiEndpoint> Endpoints { get; set; }

        [JsonProperty(Required = Required.Always)]
        public IList<TemplateMetadata> Templates { get; set; }

        [JsonProperty(Required = Required.Always)]
        public IDictionary<string, string> DatabaseConnections { get; set; }
    }
}