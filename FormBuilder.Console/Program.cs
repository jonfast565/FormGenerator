using FormBuilder.Console.Implementation;
using FormBuilder.Console.Interfaces;

namespace FormBuilder.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IModelBuilder b = new ModelBuilder();
            var templatePackage = b.Build();
            ITemplateBuilder b2 = new TemplateBuilder();
            b2.BuildTemplates(templatePackage);
        }
    }
}