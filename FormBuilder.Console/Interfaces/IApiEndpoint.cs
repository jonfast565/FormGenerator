using System.Collections.Generic;
using System.Data;
using FormBuilder.Console.Implementation;

namespace FormBuilder.Console.Interfaces
{
    public interface IApiEndpoint : IScriptHeader
    {
        string Name { get; set; }
        SqlQuery SqlQuery { get; set; }
    }
}