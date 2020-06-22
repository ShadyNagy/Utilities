using System;
using System.Collections.Generic;
using System.Text;

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
        public string Value { get; set; }
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
        Contains,
        NotContains,
        Equal,
        NotEqual,
        StartWith,
        EndWith
    }

    public enum Operator
    {
        And,
        Or,
    }
}
