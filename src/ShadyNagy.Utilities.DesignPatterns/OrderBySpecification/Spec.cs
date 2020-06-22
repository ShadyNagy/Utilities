using System;
using System.Linq.Expressions;

namespace ShadyNagy.Utilities.DesignPatterns.OrderBySpecification
{
    // Just an alias of Specification
    public sealed class Spec<TEntity> : Specification<TEntity>
        where TEntity : class
    {
        public Spec()
        {
        }

        public Spec(Expression<Func<TEntity, bool>> expression)
            : base(expression)
        {
        }
    }
}
