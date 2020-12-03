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
                if (_filters == null)
                {
                    return;
                }
                foreach (var filter in _filters)
                {
                    if (filter == null)
                    {
                        continue;
                    }
                    foreach (var condition in filter.Conditions)
                    {
                        if (condition?.Value == null || condition.Value.GetType() != typeof(JsonElement))
                        {
                            continue;
                        }

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
