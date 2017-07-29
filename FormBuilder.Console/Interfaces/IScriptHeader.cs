using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormBuilder.Console.Interfaces
{
    public interface IScriptHeader
    {
        object GetScriptHeader(IApiPackage apiPackage);
    }
}
