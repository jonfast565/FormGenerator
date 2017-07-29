using System.Configuration;
using FormBuilder.Console.Extensions;
using FormBuilder.Console.Interfaces;

namespace FormBuilder.Console.Implementation
{
    public class ModelBuilder : IModelBuilder
    {
        public ITemplatePackage Build()
        {
            return ConfigurationManager.AppSettings["ModelLocation"]
                .DeserializeJsonFileTo<TemplatePackage>();
        }
    }
}