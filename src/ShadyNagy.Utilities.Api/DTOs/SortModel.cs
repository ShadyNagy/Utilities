namespace ShadyNagy.Utilities.Api.DTOs
{
    public class SortModel
    {
        public SortOrder Order { get; set; }
        public string FieldName { get; set; }
    }

    public enum SortOrder
    {
        Asc,
        Desc
    }
}
