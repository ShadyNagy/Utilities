using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ShadyNagy.Utilities.DesignPatterns.Specification
{
    internal sealed class NotSpecification<T, T2> : Specification<T, T2>
    {

        private readonly Specification<T, T2> _specification;

        public NotSpecification(Specification<T, T2> specification)
        {
            _specification = specification;
        }

        public override Expression<Func<T, T2>> ToExpression()
        {
            Expression<Func<T, T2>> expression = _specification.ToExpression();
            //UnaryExpression notExpression = Expression.Not(expression.Body);

            //return Expression.Lambda<Func<T, bool>>(notExpression, expression.Parameters.Single());

            var paramExpr = Expression.Parameter(typeof(T));

            //var rightVisitor = new ReplaceExpressionVisitor(expression.Parameters[0], paramExpr);
            //var right = rightVisitor.Visit(expression.Body);

            return Expression.Lambda<Func<T, T2>>(
                Expression.Not(expression), paramExpr);
        }
    }
}
