using FormBuilder.Console.Enums;

namespace FormBuilder.Console.Interfaces
{
    public interface ITemplateMetadata
    {
        string Name { get; set; }
        string OutputFolder { get; set; }
        string Path { get; set; }
        string Filename { get; set; }
        TemplateScope Scope { get; set; }
    }
}