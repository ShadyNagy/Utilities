using System.Collections.Generic;
using ShadyNagy.Utilities.Api.DTOs;
using ShadyNagy.Utilities.DesignPatterns.Specification;
using ShadyNagy.Utilities.DesignPatterns.Tests.Models;
using Xunit;

namespace ShadyNagy.Utilities.DesignPatterns.Tests
{
    public class SortSpecificationTest
    {
        [Fact]
        public void AscTest()
        {
            var sorts = new List<SortModel>();

            var sort = new SortModel {FieldName = "Name", Order = SortOrder.Asc};

            sorts.Add(sort);

            var spec = SortSpecification<Customer>.Create(sorts);
            var expression = spec.ToExpression();

            Assert.Equal("x.Name.Contains(\"s\")", expression.Body.ToString());
        }

       
    }

}
