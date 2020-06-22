using System;
using System.Linq.Expressions;

namespace ShadyNagy.Utilities.DesignPatterns.OrderBySpecification
{
    internal class DummySpecification<TEntity> : IQuerySpecification<TEntity>
        where TEntity : class
    {
        public Configuration<TEntity> Internal => new Configuration<TEntity>(this);

        public Expression<Func<TEntity, bool>> AsExpression() => null;

        public Func<TEntity, bool> AsFunc() => null;
    }
}
