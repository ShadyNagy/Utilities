using System;
using System.Linq.Expressions;

namespace ShadyNagy.Utilities.DesignPatterns.Specification
{

    public abstract class Specification<T, T2>
    {

        public static readonly Specification<T, T2> All = new IdentitySpecification<T, T2>();


        public T2 IsSatisfiedBy(T entity)
        {
            Func<T, T2> predicate = ToExpression().Compile();
            return predicate(entity);
        }


        public abstract Expression<Func<T, T2>> ToExpression();

        public Specification<T, T2> And(Specification<T, T2> specification)
        {
            if (this == All)
                return specification;
            if (specification == All)
                return this;

            return new AndSpecification<T, T2>(this, specification);
        }

        public Specification<T, T2> Or(Specification<T, T2> specification)
        {
            if (this == All || specification == All)
                return All;

            return new OrSpecification<T, T2>(this, specification);
        }

        public Specification<T, T2> Not()
        {
            return new NotSpecification<T, T2>(this);
        }
    }
}
