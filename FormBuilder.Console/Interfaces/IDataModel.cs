using System.Collections.Generic;

namespace FormBuilder.Console.Interfaces
{
    public interface IDataModel
    {
        IList<IDataField> Fields { get; set; }
    }
}