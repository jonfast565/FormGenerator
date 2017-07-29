using System.Collections.Generic;
using FormBuilder.Console.Interfaces;
using Newtonsoft.Json;

namespace FormBuilder.Console.Implementation
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TableDataModel : IDataModel
    {
        [JsonProperty(Required = Required.Always)]
        public IList<IDataField> Fields { get; set; }
    }
}