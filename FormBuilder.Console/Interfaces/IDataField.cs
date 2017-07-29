namespace FormBuilder.Console.Interfaces
{
    public interface IDataField
    {
        string Name { get; set; }
        string Type { get; set; }
        bool IsNullable { get; set; }
    }
}