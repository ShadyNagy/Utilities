using System.Collections.Generic;
#if NETFRAMEWORK
#else
using System.Text.Json;
#endif

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

#if NETFRAMEWORK
        public List<FilterModel> Filters { get; set; }
#else
        private List<FilterModel> _filters;
        public List<FilterModel> Filters
        {
            get => _filters;
            set
            {
                _filters = value;
                if (value.GetType() != typeof(JsonElement))
                {
                    return;
                }
                foreach (var filter in _filters)
                {
                    foreach (var condition in filter.Conditions)
                    {
                        var temp = (JsonElement)condition.Value;
                        if (temp.ValueKind == JsonValueKind.Number)
                        {
                            condition.Value = temp.GetDecimal();
                        }
                        else if (temp.ValueKind == JsonValueKind.String)
                        {
                            condition.Value = temp.GetString();
                        }
                        else if (temp.ValueKind == JsonValueKind.Null)
                        {
                            condition.Value = null;
                        }
                        else if (temp.ValueKind == JsonValueKind.False)
                        {
                            condition.Value = false;
                        }
                        else if (temp.ValueKind == JsonValueKind.True)
                        {
                            condition.Value = true;
                        }
                    }
                }
            }
        }
#endif
    }



}
