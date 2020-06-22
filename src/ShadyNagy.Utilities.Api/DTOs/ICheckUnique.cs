namespace ShadyNagy.Utilities.Api.DTOs
{
    public interface ICheckUnique
    {
        string FieldName { get; set; }
        string Value { get; set; }
        string Id { get; set; }
    }
}
