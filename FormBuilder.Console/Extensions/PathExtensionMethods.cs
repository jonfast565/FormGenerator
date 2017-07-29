using System.IO;
using HandlebarsDotNet;
using Newtonsoft.Json;

namespace FormBuilder.Console.Extensions
{
    public static class PathExtensionMethods
    {
        public static string GetFileStringFromPath(this string path)
        {
            return File.ReadAllText(path);
        }

        public static byte[] GetFileBytesFromPath(this string path)
        {
            return File.ReadAllBytes(path);
        }

        public static string TemplateFromPath(this string path, object templateHeader)
        {
            var templateFile = path.GetFileStringFromPath();
            var template = Handlebars.Compile(templateFile);
            return template(templateHeader);
        }

        public static string TemplateFromString(this string templateString, object templateHeader)
        {
            var template = Handlebars.Compile(templateString);
            return template(templateHeader);
        }

        public static T DeserializeJsonFileTo<T>(this string path)
        {
            var file = path.GetFileStringFromPath();
            return JsonConvert.DeserializeObject<T>(file);
        }
    }
}