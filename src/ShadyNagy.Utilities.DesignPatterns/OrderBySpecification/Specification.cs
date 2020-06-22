using System;
using System.Linq;
using System.Linq.Expressions;

namespace ShadyNagy.Utilities.DesignPatterns.OrderBySpecification
{
    public class Specification<TEntity> : QuerySpecification<TEntity>
        where TEntity : class
    {
        public Specification()
        {
        }

        public Specification(Expression<Func<TEntity, bool>> expression)
            : base(expression)
        {
        }

        public static ISpecification<TEntity> New()
        {
            return new Specification<TEntity>();
        }

        public static ISpecification<TEntity> New(Expression<Func<TEntity, bool>> expression)
        {
            return new Specification<TEntity>(expression);
        }
    }
}
