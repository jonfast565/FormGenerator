using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using FormBuilder.Console.Extensions;
using FormBuilder.Console.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FormBuilder.Console.Implementation
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ApiEndpoint : IApiEndpoint
    {
        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty(Required = Required.Always)]
        public SqlQuery SqlQuery { get; set; }

        public object GetScriptHeader(IApiPackage package)
        {
            var parameters = SqlQuery.CommandType == CommandType.StoredProcedure
                ? SqlQuery.DeriveParameters(package).Cast<QueryParameter>().ToList()
                : null;

            var query = SqlQuery
                .GetQuery()
                .Replace("\"", "\\\"")
                .Replace("\r\n", "\n")
                .Replace("\n", " ")
                .Replace("\t", " ")
                .RemoveExcessSpaces();

            var fields = SqlQuery.GetSqlDataModel(package, parameters).Fields;

            return new
            {
                PackageName = package.Name,
                package.ApiRootUrl,
                Name,
                SqlQuery.Connection,
                Query = query,
                SqlQuery.CommandType,
                Fields = fields,
                Parameters = parameters
            };
        }
    }
}