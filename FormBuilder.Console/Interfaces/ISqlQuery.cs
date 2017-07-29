using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormBuilder.Console.Enums;
using FormBuilder.Console.Implementation;

namespace FormBuilder.Console.Interfaces
{
    public interface ISqlQuery
    {
        string Query { get; set; }
        SqlQuerySource QuerySource { get; set; }
        CommandType CommandType { get; set; }
        string Connection { get; set; }

        IDataModel GetSqlDataModel(IApiPackage package, IList<QueryParameter> parameters);
        IList<IQueryParameter> DeriveParameters(IApiPackage package);
    }
}
