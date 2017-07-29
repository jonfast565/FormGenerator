using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormBuilder.Console.Enums;
using FormBuilder.Console.Extensions;
using FormBuilder.Console.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FormBuilder.Console.Implementation
{
    public class SqlQuery : ISqlQuery
    {
        [JsonProperty(Required = Required.Always)]
        public string Query { get; set; }

        [JsonProperty(Required = Required.Always)]
        public SqlQuerySource QuerySource { get; set; }

        [JsonProperty(Required = Required.Always)]
        [JsonConverter(typeof(StringEnumConverter))]
        public CommandType CommandType { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Connection { get; set; }

        public IDataModel GetSqlDataModel(IApiPackage package, 
            IList<QueryParameter> derivedQueryParameters)
        {
            return package.DatabaseConnections[Connection]
                .BuildAndOpenConnection()
                .RunSqlQuery(GetQuery(), CommandType, derivedQueryParameters)
                .GetFields()
                .GetDataModel();
        }

        public IList<IQueryParameter> DeriveParameters(IApiPackage package)
        {
            if (CommandType != CommandType.StoredProcedure)
                throw new ArgumentException("Command type is not stored proc");
            if (QuerySource != SqlQuerySource.Inline)
                throw new ArgumentException("Query source is not inline");
            return package.DatabaseConnections[Connection]
                .BuildAndOpenConnection()
                .DeriveStoredProcedureParameters(Query);
        }

        public string GetQuery()
        {
            switch (QuerySource)
            {
                case SqlQuerySource.File:
                    return Query.GetFileStringFromPath();
                case SqlQuerySource.Inline:
                    return Query;
                default:
                    throw new InvalidEnumArgumentException("QuerySource");
            }
        }
    }
}
