using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormBuilder.Console.Interfaces
{
    public interface IQueryParameter
    {
        string Name { get; set; }
        Type Type { get; set; }
        string TypeName { get; set; }
        SqlDbType DatabaseType { get; set; }
        string DatabaseParameterName { get; set; }
        bool IsNullable { get; set; }
        int? OptionalLength { get; set; }
        ParameterDirection ParameterDirection { get; set; }
    }
}
