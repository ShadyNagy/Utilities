using System.Collections.Generic;
using ShadyNagy.Utilities.Api.DTOs;
using ShadyNagy.Utilities.DesignPatterns.Specification;
using ShadyNagy.Utilities.DesignPatterns.Tests.Models;
using Xunit;

namespace ShadyNagy.Utilities.DesignPatterns.Tests
{
    public class FilterSpecificationTest
    {
        [Fact]
        public void SpecificationContainsTest()
        {
            var filters = new List<FilterModel>();

            var filter = new FilterModel {FieldName = "Name", FilterType = FilterType.Text};

            var condition = new Condition
            {
                FilterType = FilterType.Text, ConditionType = ConditionType.Contains, Value = "s"
            };

            filter.Conditions.Add(condition);
            filters.Add(filter);

            var spec = FilterSpecification<Customer>.Create(filters);
            var expression = spec.ToExpression();

            Assert.Equal("x.Name.Contains(\"s\")", expression.Body.ToString());
        }

        [Fact]
        public void SpecificationEqualTest()
        {
            var filters = new List<FilterModel>();

            var filter = new FilterModel {FieldName = "Name", FilterType = FilterType.Text};

            var condition = new Condition
            {
                FilterType = FilterType.Text, ConditionType = ConditionType.Equals, Value = "s"
            };

            filter.Conditions.Add(condition);
            filters.Add(filter);

            var spec = FilterSpecification<Customer>.Create(filters);
            var expression = spec.ToExpression();

            Assert.Equal("(x.Name == \"s\")", expression.Body.ToString());
        }

        [Fact]
        public void SpecificationNotEqualTest()
        {
            var filters = new List<FilterModel>();

            var filter = new FilterModel {FieldName = "Name", FilterType = FilterType.Text};

            var condition = new Condition
            {
                FilterType = FilterType.Text, ConditionType = ConditionType.NotEqual, Value = "s"
            };

            filter.Conditions.Add(condition);
            filters.Add(filter);

            var spec = FilterSpecification<Customer>.Create(filters);
            var expression = spec.ToExpression();

            Assert.Equal("(x.Name != \"s\")", expression.Body.ToString());
        }

        [Fact]
        public void SpecificationEndsWithTest()
        {
            var filters = new List<FilterModel>();

            var filter = new FilterModel {FieldName = "Name", FilterType = FilterType.Text};

            var condition = new Condition
            {
                FilterType = FilterType.Text, ConditionType = ConditionType.EndsWith, Value = "s"
            };

            filter.Conditions.Add(condition);
            filters.Add(filter);

            var spec = FilterSpecification<Customer>.Create(filters);
            var expression = spec.ToExpression();

            Assert.Equal("x.Name.EndsWith(\"s\")", expression.Body.ToString());
        }

        [Fact]
        public void SpecificationStartsWithTest()
        {
            var filters = new List<FilterModel>();

            var filter = new FilterModel {FieldName = "Name", FilterType = FilterType.Text};

            var condition = new Condition
            {
                FilterType = FilterType.Text, ConditionType = ConditionType.StartsWith, Value = "s"
            };

            filter.Conditions.Add(condition);
            filters.Add(filter);

            var spec = FilterSpecification<Customer>.Create(filters);
            var expression = spec.ToExpression();

            Assert.Equal("x.Name.StartsWith(\"s\")", expression.Body.ToString());
        }

        [Fact]
        public void SpecificationGreaterThanTest()
        {
            var filters = new List<FilterModel>();

            var filter = new FilterModel {FieldName = "Age", FilterType = FilterType.Text};

            var condition = new Condition
            {
                FilterType = FilterType.Text, ConditionType = ConditionType.GreaterThan, Value = 5
            };

            filter.Conditions.Add(condition);
            filters.Add(filter);

            var spec = FilterSpecification<Customer>.Create(filters);
            var expression = spec.ToExpression();

            Assert.Equal("(x.Age > 5)", expression.Body.ToString());
        }

        [Fact]
        public void SpecificationGreaterThanOrEqualTest()
        {
            var filters = new List<FilterModel>();

            var filter = new FilterModel {FieldName = "Age", FilterType = FilterType.Text};

            var condition = new Condition
            {
                FilterType = FilterType.Text, ConditionType = ConditionType.GreaterThanOrEqual, Value = 5
            };

            filter.Conditions.Add(condition);
            filters.Add(filter);

            var spec = FilterSpecification<Customer>.Create(filters);
            var expression = spec.ToExpression();

            Assert.Equal("(x.Age >= 5)", expression.Body.ToString());
        }

        [Fact]
        public void SpecificationLessThanTest()
        {
            var filters = new List<FilterModel>();

            var filter = new FilterModel {FieldName = "Age", FilterType = FilterType.Text};

            var condition = new Condition
            {
                FilterType = FilterType.Text, ConditionType = ConditionType.LessThan, Value = 5
            };

            filter.Conditions.Add(condition);
            filters.Add(filter);

            var spec = FilterSpecification<Customer>.Create(filters);
            var expression = spec.ToExpression();

            Assert.Equal("(x.Age < 5)", expression.Body.ToString());
        }

        [Fact]
        public void SpecificationLessThanOrEqualTest()
        {
            var filters = new List<FilterModel>();

            var filter = new FilterModel {FieldName = "Age", FilterType = FilterType.Text};

            var condition = new Condition
            {
                FilterType = FilterType.Text, ConditionType = ConditionType.LessThanOrEqual, Value = 5
            };

            filter.Conditions.Add(condition);
            filters.Add(filter);

            var spec = FilterSpecification<Customer>.Create(filters);
            var expression = spec.ToExpression();

            Assert.Equal("(x.Age <= 5)", expression.Body.ToString());
        }

        [Fact]
        public void SpecificationNestedLessThanOrEqualIntTest()
        {
            var filters = new List<FilterModel>();

            var filter = new FilterModel { FieldName = "Payment.Money", FilterType = FilterType.Text };

            var condition = new Condition
            {
                FilterType = FilterType.Text,
                ConditionType = ConditionType.LessThanOrEqual,
                Value = 5
            };

            filter.Conditions.Add(condition);
            filters.Add(filter);

            var spec = FilterSpecification<Order>.Create(filters);
            var expression = spec.ToExpression();

            Assert.Equal("(x.Payment.Money <= 5)", expression.Body.ToString());
        }

        [Fact]
        public void SpecificationNestedLessThanOrEqualDecimalTest()
        {
            var filters = new List<FilterModel>();

            var filter = new FilterModel { FieldName = "payment.cost", FilterType = FilterType.Text };

            var condition = new Condition
            {
                FilterType = FilterType.Text,
                ConditionType = ConditionType.LessThanOrEqual,
                Value = 5
            };

            filter.Conditions.Add(condition);
            filters.Add(filter);

            var spec = FilterSpecification<Order>.Create(filters);
            var expression = spec.ToExpression();

            Assert.Equal("(x.Payment.Cost <= Convert(5, Decimal))", expression.Body.ToString());
        }

        [Fact]
        public void SpecificationStringFromListTest()
        {
            var filters = new List<FilterModel>();

            var filter = new FilterModel { FieldName = "Orders[Payment.Id]", FilterType = FilterType.Text };

            var condition = new Condition
            {
                FilterType = FilterType.Text,
                ConditionType = ConditionType.Equals,
                Value = "value"
            };

            filter.Conditions.Add(condition);
            filters.Add(filter);

            var spec = FilterSpecification<Customer>.Create(filters);
            var expression = spec.ToExpression();

            Assert.Equal("x.Orders.Any(i => (i.Payment.Id == \"value\"))", expression.Body.ToString());
        }

        [Fact]
        public void SpecificationBadPropertyEqualTest()
        {
            var filters = new List<FilterModel>();

            var filter = new FilterModel { FieldName = "bad", FilterType = FilterType.Text };

            var condition = new Condition
            {
                FilterType = FilterType.Text,
                ConditionType = ConditionType.Equals,
                Value = "s"
            };

            filter.Conditions.Add(condition);
            filters.Add(filter);

            var spec = FilterSpecification<Customer>.Create(filters);
            var expression = spec.ToExpression();

            Assert.Equal("Convert(Convert(ToBoolean(True), Object), Boolean)", expression.Body.ToString());
        }
    }

}
