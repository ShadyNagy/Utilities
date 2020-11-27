using System.Collections.Generic;

namespace ShadyNagy.Utilities.Api.DTOs
{
    public class FilterModel
    {
        public FilterModel()
        {
            Conditions = new List<Condition>();
        }
        public string FieldName { get; set; }
        public FilterType FilterType { get; set; }
        public List<Condition> Conditions { get; set; }
    }

    public class Condition
    {
        public object Value { get; set; }
        public FilterType FilterType { get; set; }
        public ConditionType ConditionType { get; set; }
        public Operator OperatorWithNext { get; set; }
    }

    public enum FilterType
    {
        Text,
    }

    public enum ConditionType
    {
        Equals,
        NotEqual,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,
        Contains,
        StartsWith,
        EndsWith
    }

    public enum Operator
    {
        And,
        Or,
    }
}
