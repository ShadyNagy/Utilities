namespace ShadyNagy.Utilities.Api.DTOs
{
    public interface ISearchParameters
    {
        int Page { get; set; }
        string FieldName { get; set; }
        string Value { get; set; }
    }
}
