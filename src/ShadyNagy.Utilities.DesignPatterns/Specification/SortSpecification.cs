using System.Collections.Generic;
using ShadyNagy.Utilities.Api.DTOs;

namespace ShadyNagy.Utilities.DesignPatterns.Specification
{
    public class SortSpecification<TModel> : Specification<TModel, object>
    {
        public SortSpecification(string propertyName, SortOrder sortOrder)
        {
            PropertyName = propertyName;
            SortOrder = sortOrder;
        }

        public static Specification<TModel, object> Create(List<SortModel> sorts)
        {
            var spec = All;

            foreach (var sort in sorts)
            {
                var sortOrder = (SortOrder)(int)sort.Order;
                var specToAdd = new SortSpecification<TModel>(sort.FieldName, sortOrder);

                spec = spec.And(specToAdd);
            }

            return spec;
        }

    }
}
