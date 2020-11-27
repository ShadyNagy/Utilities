using System.Collections.Generic;
using ShadyNagy.Utilities.Api.DTOs;

namespace ShadyNagy.Utilities.DesignPatterns.Specification
{
    public class FilterSpecification<TModel> : Specification<TModel, bool>
    {
        public FilterSpecification(string propertyName, FilterOperator filterOperator, object value)
        {
            PropertyName = propertyName;
            FilterOperator = filterOperator;
            Value = value;
        }

        public static Specification<TModel, bool> Create(List<FilterModel> filters)
        {
            var spec = All;

            foreach (var filter in filters)
            {
                foreach (var condition in filter.Conditions)
                {
                    var filterType = (FilterOperator)(int)condition.ConditionType;
                    var specToAdd = new FilterSpecification<TModel>(filter.FieldName, filterType, condition.Value);

                    if (condition.OperatorWithNext == Operator.And)
                    {
                        spec = spec.And(specToAdd);
                    }
                    else
                    {
                        spec = spec.Or(specToAdd);
                    }
                }
            }

            return spec;
        }

    }
}
