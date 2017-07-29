using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormBuilder.Console.Implementation;
using FormBuilder.Console.Interfaces;
using Microsoft.CSharp;

namespace FormBuilder.Console.Extensions
{
    public static class SqlExtensionMethods
    {
        public static SqlConnection BuildAndOpenConnection(this string connectionString)
        {
            var conn = new SqlConnection(connectionString);
            conn.Open();
            return conn;
        }

        public static DataTable RunSqlQuery(this SqlConnection connection, 
            string query, 
            CommandType type = CommandType.Text, 
            IList<QueryParameter> derivedQueryParameters = null)
        {
            using (var command = new SqlCommand(query, connection))
            {
                command.CommandType = type;
                if (derivedQueryParameters != null && type == CommandType.StoredProcedure)
                    foreach (var parameter in derivedQueryParameters)
                        command.Parameters.Add(
                            parameter.DeriveSqlParameter());
                var table = new DataTable();
                var adapter = new SqlDataAdapter(command);
                adapter.Fill(table);
                return table;
            }
        }

        public static SqlParameter DeriveSqlParameter(
            this QueryParameter parameter)
        {
            return new SqlParameter(
                parameter.DatabaseParameterName, 
                parameter.Type.FromType())
            {
                Direction = parameter.ParameterDirection,
                Value = parameter.Type.FromType().GetTestValue(),
                IsNullable = parameter.IsNullable
            };
        }

        public static IList<IQueryParameter> DeriveStoredProcedureParameters(
            this SqlConnection connection, 
            string storedProcedureName)
        {
            var compiler = new CSharpCodeProvider();
            var paramList = new List<IQueryParameter>();
            using (var cmd = new SqlCommand(storedProcedureName, connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Prepare();
                SqlCommandBuilder.DeriveParameters(cmd);

                paramList.AddRange(
                    from SqlParameter p in cmd.Parameters
                    where p.ParameterName.ToLowerInvariant() != "@return_value"
                    select new QueryParameter
                    {
                        Name = p.ParameterName.Substring(1).CamelCase(),
                        DatabaseParameterName = p.ParameterName,
                        IsNullable = p.IsNullable,
                        Type = p.DbType.FromDbType(),
                        TypeName = new Func<string>(() =>
                        {
                            var type = p.DbType.FromDbType();
                            var compilerTypeRef = new CodeTypeReference(type);
                            var actualType = compiler.GetTypeOutput(compilerTypeRef);
                            return actualType;
                        })(),
                        DatabaseType = p.SqlDbType,
                        OptionalLength = p.Size > 0 ? p.Size : default(int),
                        ParameterDirection = p.Direction
                    });
            }
            return paramList;
        }

        public static IList<IDataField> GetFields(this DataTable table)
        {
            var compiler = new CSharpCodeProvider();
            var fieldList = new List<IDataField>();
            var columnsLength = table.Columns.Count;
            for (var i = 0; i < columnsLength; i++)
            {
                var type = table.Columns[i].DataType;
                var compilerTypeRef = new CodeTypeReference(type);
                var actualType = compiler.GetTypeOutput(compilerTypeRef);
                fieldList.Add(new DataField
                {
                    Name = table.Columns[i].ColumnName,
                    Type = actualType,
                    IsNullable = type.IsPrimitive
                });
            }
            return fieldList;
        }

        public static IDataModel GetDataModel(this IList<IDataField> fields)
        {
            return new TableDataModel
            {
                Fields = fields
            };
        }

        public static object GetTestValue(this DbType type)
        {
            var typeMap = new Dictionary<DbType, object>
            {
                [DbType.Byte] = 0x1,
                [DbType.SByte] = 0x1,
                [DbType.Int16] = 1,
                [DbType.UInt16] = 1,
                [DbType.Int32] = 1,
                [DbType.UInt32] = 1U,
                [DbType.Int64] = 1L,
                [DbType.UInt64] = 1UL,
                [DbType.Single] = 1F,
                [DbType.Double] = 1D,
                [DbType.Decimal] = 1M,
                [DbType.Boolean] = true,
                [DbType.StringFixedLength] = '0',
                [DbType.AnsiStringFixedLength] = '0',
                [DbType.String] = "0",
                [DbType.AnsiString] = "0",
                [DbType.Guid] = Guid.NewGuid(),
                [DbType.DateTime] = DateTime.Now,
                [DbType.DateTimeOffset] = new DateTimeOffset(DateTime.Now),
                [DbType.Binary] = new byte[] { 0x0 }
            };
            return typeMap[type];
        }

        [Obsolete]
        public static SqlDbType ToSqlDbType(this DbType type)
        {
            var typeMap = new Dictionary<DbType, SqlDbType>
            {
                [DbType.Byte] = SqlDbType.TinyInt,
                [DbType.SByte] = SqlDbType.TinyInt,
                [DbType.Int16] = SqlDbType.SmallInt,
                [DbType.UInt16] = SqlDbType.SmallInt,
                [DbType.Int32] = SqlDbType.Int,
                [DbType.UInt32] = SqlDbType.Int,
                [DbType.Int64] = SqlDbType.BigInt,
                [DbType.UInt64] = SqlDbType.BigInt,
                [DbType.Single] = SqlDbType.Real,
                [DbType.Double] = SqlDbType.Float,
                [DbType.Decimal] = SqlDbType.Decimal,
                [DbType.Boolean] = SqlDbType.Bit,
                [DbType.StringFixedLength] = SqlDbType.NChar,
                [DbType.AnsiStringFixedLength] = SqlDbType.NChar,
                [DbType.String] = SqlDbType.NVarChar,
                [DbType.AnsiString] = SqlDbType.NVarChar,
                [DbType.Guid] = SqlDbType.UniqueIdentifier,
                [DbType.DateTime] = SqlDbType.DateTime2,
                [DbType.DateTimeOffset] = SqlDbType.DateTimeOffset,
                [DbType.Binary] = SqlDbType.VarBinary,
            };
            return typeMap[type];
        }

        public static Type FromDbType(this DbType type)
        {
            var typeMap = new Dictionary<DbType, Type>
            {
                [DbType.Byte] = typeof(byte),
                [DbType.SByte] = typeof(sbyte),
                [DbType.Int16] = typeof(short),
                [DbType.UInt16] = typeof(ushort),
                [DbType.Int32] = typeof(int) ,
                [DbType.UInt32] = typeof(uint),
                [DbType.Int64] = typeof(long),
                [DbType.UInt64] = typeof(ulong),
                [DbType.Single] = typeof(float),
                [DbType.Double] = typeof(double),
                [DbType.Decimal] = typeof(decimal),
                [DbType.Boolean] = typeof(bool),
                [DbType.StringFixedLength] = typeof(char),
                [DbType.AnsiStringFixedLength] = typeof(char),
                [DbType.String] = typeof(string),
                [DbType.AnsiString] = typeof(string),
                [DbType.Guid] = typeof(Guid),
                [DbType.DateTime] = typeof(DateTime),
                [DbType.DateTimeOffset] = typeof(DateTimeOffset),
                [DbType.Binary] = typeof(byte[]),
            };
            return typeMap[type];
        }

        public static DbType FromType(this Type type)
        {
            var typeMap = new Dictionary<Type, DbType>
            {
                [typeof(byte)] = DbType.Byte,
                [typeof(sbyte)] = DbType.SByte,
                [typeof(short)] = DbType.Int16,
                [typeof(ushort)] = DbType.UInt16,
                [typeof(int)] = DbType.Int32,
                [typeof(uint)] = DbType.UInt32,
                [typeof(long)] = DbType.Int64,
                [typeof(ulong)] = DbType.UInt64,
                [typeof(float)] = DbType.Single,
                [typeof(double)] = DbType.Double,
                [typeof(decimal)] = DbType.Decimal,
                [typeof(bool)] = DbType.Boolean,
                [typeof(string)] = DbType.String,
                [typeof(char)] = DbType.StringFixedLength,
                [typeof(Guid)] = DbType.Guid,
                [typeof(DateTime)] = DbType.DateTime,
                [typeof(DateTimeOffset)] = DbType.DateTimeOffset,
                [typeof(byte[])] = DbType.Binary,
                // nullable types
                [typeof(byte?)] = DbType.Byte,
                [typeof(sbyte?)] = DbType.SByte,
                [typeof(short?)] = DbType.Int16,
                [typeof(ushort?)] = DbType.UInt16,
                [typeof(int?)] = DbType.Int32,
                [typeof(uint?)] = DbType.UInt32,
                [typeof(long?)] = DbType.Int64,
                [typeof(ulong?)] = DbType.UInt64,
                [typeof(float?)] = DbType.Single,
                [typeof(double?)] = DbType.Double,
                [typeof(decimal?)] = DbType.Decimal,
                [typeof(bool?)] = DbType.Boolean,
                [typeof(char?)] = DbType.StringFixedLength,
                [typeof(Guid?)] = DbType.Guid,
                [typeof(DateTime?)] = DbType.DateTime,
                [typeof(DateTimeOffset?)] = DbType.DateTimeOffset
            };
            return typeMap[type];
        }
    }
}
