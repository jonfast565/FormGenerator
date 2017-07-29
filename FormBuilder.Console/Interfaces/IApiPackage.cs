using System.Collections.Generic;
using FormBuilder.Console.Implementation;

namespace FormBuilder.Console.Interfaces
{
    public interface IApiPackage
    {
        string Name { get; set; }
        string OutputFolder { get; set; }
        string ApiRootUrl { get; set; }
        IList<ApiEndpoint> Endpoints { get; set; }
        IList<TemplateMetadata> Templates { get; set; }
        IDictionary<string, string> DatabaseConnections { get; set; }
    }
}