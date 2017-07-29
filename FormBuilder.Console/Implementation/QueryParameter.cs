using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormBuilder.Console.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FormBuilder.Console.Implementation
{
    [JsonObject(MemberSerialization.OptIn)]
    public class QueryParameter : IQueryParameter
    {
        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty(Required = Required.Always)]
        public Type Type { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string TypeName { get; set; }

        public SqlDbType DatabaseType { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string DatabaseParameterName { get; set; }

        [JsonProperty(Required = Required.Always)]
        public bool IsNullable { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public int? OptionalLength { get; set; }

        [JsonProperty(Required = Required.Always)]
        [JsonConverter(typeof(StringEnumConverter))]
        public ParameterDirection ParameterDirection { get; set; }
    }
}
