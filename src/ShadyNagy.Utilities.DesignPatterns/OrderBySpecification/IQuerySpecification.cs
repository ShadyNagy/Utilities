using System;
using System.Linq.Expressions;

namespace ShadyNagy.Utilities.DesignPatterns.OrderBySpecification
{
    public interface IQuerySpecification<TEntity> : ISpecification<TEntity>
        where TEntity : class
    {
        Expression<Func<TEntity, bool>> AsExpression();

        Func<TEntity, bool> AsFunc();
    }
}
