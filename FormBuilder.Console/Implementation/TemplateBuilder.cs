using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using FormBuilder.Console.Enums;
using FormBuilder.Console.Extensions;
using FormBuilder.Console.Interfaces;

namespace FormBuilder.Console.Implementation
{
    public class TemplateBuilder : ITemplateBuilder
    {
        public void BuildTemplates(ITemplatePackage package)
        {
            var headerObjects = 
                package.ApiPackage.Endpoints
                .Select(endpoint => endpoint.GetScriptHeader(package.ApiPackage))
                .ToList();

            foreach (var template in package.ApiPackage.Templates)
            {
                foreach (var endpoint in headerObjects)
                {
                    if (template.Scope != TemplateScope.ApiEndpoint) continue;
                    WriteToDisk(package.ApiPackage, template, endpoint);
                }

                if (template.Scope != TemplateScope.ApiPackage) continue;
                var globalHeader = GetScriptHeader(package.ApiPackage, headerObjects);
                WriteToDisk(package.ApiPackage, template, globalHeader);
            }
        }

        private static void WriteToDisk(IApiPackage package, 
            ITemplateMetadata template, 
            object header)
        {
            var templateResult = template.Path.TemplateFromPath(header);
            var templateFilename = template.Filename.TemplateFromString(header);
            var realOutputFolder = Path.Combine(package.OutputFolder, template.OutputFolder);
            if (!Directory.Exists(realOutputFolder))
                Directory.CreateDirectory(realOutputFolder);
            var path = Path.Combine(package.OutputFolder, template.OutputFolder, 
                templateFilename);
            File.WriteAllText(path, templateResult);
        }

        public static object GetScriptHeader(IApiPackage apiPackage, 
            IList<object> endpoints)
        {
            return new
            {
                SolutionGuid = "{" + Guid.NewGuid() + "}",
                ProjectGuid = "{" + Guid.NewGuid() + "}",
                apiPackage.Name,
                Endpoints = endpoints,
                apiPackage.ApiRootUrl,
                apiPackage.DatabaseConnections
            };
        }
    }
}