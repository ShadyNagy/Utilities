using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ShadyNagy.Utilities.DesignPatterns.Specification
{
    internal sealed class OrSpecification<T, T2> : Specification<T, T2>
    {
        private readonly Specification<T, T2> _left;
        private readonly Specification<T, T2> _right;

        public OrSpecification(Specification<T, T2> left, Specification<T, T2> right)
        {
            _right = right;
            _left = left;
        }

        public override Expression<Func<T, T2>> ToExpression()
        {
            var leftExpression = _left.ToExpression();
            var rightExpression = _right.ToExpression();

            //BinaryExpression orExpression = Expression.OrElse(leftExpression.Body, rightExpression.Body);

            //return Expression.Lambda<Func<T, bool>>(orExpression, leftExpression.Parameters.Single());

            var paramExpr = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(leftExpression.Parameters[0], paramExpr);
            var left = leftVisitor.Visit(leftExpression.Body);

            var rightVisitor = new ReplaceExpressionVisitor(rightExpression.Parameters[0], paramExpr);
            var right = rightVisitor.Visit(rightExpression.Body);

            return Expression.Lambda<Func<T, T2>>(
                Expression.OrElse(left, right), paramExpr);
        }
    }
}
