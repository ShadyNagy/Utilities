using System.Collections.Generic;

namespace ShadyNagy.Utilities.Api.DTOs
{
    public class LookupGetRequest
    {
        public LookupGetRequest()
        {
            Filters = new List<FilterModel>();
            Sorts = new List<SortModel>();
        }
        public int? Page { get; set; }
        public int LanguageId { get; set; }
        public List<SortModel> Sorts { get; set; }
        public List<FilterModel> Filters { get; set; }
    }

    

}
