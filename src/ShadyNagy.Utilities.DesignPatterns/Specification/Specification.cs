using System;
using System.Linq.Expressions;
using System.Reflection;
using ShadyNagy.Utilities.Extensions.Expressions;

namespace ShadyNagy.Utilities.DesignPatterns.Specification
{

    public abstract class Specification<T, T2>
    {
        private static readonly Type _stringType = typeof(string);

        private static readonly MethodInfo _toStringMethod = typeof(object).GetMethod("ToString");

        private static readonly MethodInfo _containsMethod = typeof(string).GetMethod("Contains"
            , new Type[] { typeof(string) });

        private static readonly MethodInfo _endsWithMethod
            = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });

        private static readonly MethodInfo _startsWithMethod
            = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });


        public static readonly Specification<T, T2> All = new IdentitySpecification<T, T2>();
        public string PropertyName { get; set; }
        public FilterOperator FilterOperator { get; set; }
        public object Value { get; set; }

        public T2 IsSatisfiedBy(T entity)
        {
            Func<T, T2> predicate = ToExpression().Compile();
            return predicate(entity);
        }


        public virtual Expression<Func<T, T2>> ToExpression()
        {
            if (typeof(T2) == typeof(bool))
            {
                if (string.IsNullOrEmpty(PropertyName))
                {
                    return x => (T2)(object)Convert.ToBoolean(true);
                }
                else
                {
                    var param = Expression.Parameter(typeof(T));
                    return Expression.Lambda<Func<T, T2>>(GetFilter(param, PropertyName, FilterOperator, Value), param);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(PropertyName))
                {
                    return x => default;
                }
                else
                {
                    var param = Expression.Parameter(typeof(T));
                    return Expression.Lambda<Func<T, T2>>(GetPropertyGetter<T>(PropertyName), param);
                }
            }
        }

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

        public static Expression<Func<TEntity, object>> GetPropertyGetter<TEntity>(string property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));

            var param = Expression.Parameter(typeof(TEntity));
            var prop = param.GetNestedProperty(property);
            var convertedProp = Expression.Convert(prop, typeof(object));
            return Expression.Lambda<Func<TEntity, object>>(convertedProp, param);
        }

        internal static Expression GetFilter(ParameterExpression param, string property, FilterOperator op, object value)
        {
            var constant = Expression.Constant(value);
            var prop = param.GetNestedProperty(property);
            return CreateFilter(prop, op, constant);
        }

        private static Expression CreateFilter(MemberExpression prop, FilterOperator filterOperator, ConstantExpression constant)
        {
            switch (filterOperator)
            {
                case FilterOperator.Equals:
                    return Expression.Equal(prop, constant);
                case FilterOperator.GreaterThan:
                    return Expression.GreaterThan(prop, constant);
                case FilterOperator.LessThan:
                    return Expression.LessThan(prop, constant);
                case FilterOperator.Contains:
                    return Expression.Call(prop, _containsMethod, PrepareConstant(constant));
                case FilterOperator.StartsWith:
                    return Expression.Call(prop, _startsWithMethod, PrepareConstant(constant));
                case FilterOperator.EndsWith:
                    return Expression.Call(prop, _endsWithMethod, PrepareConstant(constant));
                case FilterOperator.NotEqual:
                    return Expression.NotEqual(prop, constant);
                case FilterOperator.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(prop, constant);
                case FilterOperator.LessThanOrEqual:
                    return Expression.LessThanOrEqual(prop, constant);

                default:
                    throw new NotImplementedException();
            }
        }

        private static Expression PrepareConstant(ConstantExpression constant)
        {
            if (constant.Type == _stringType)
                return constant;

            var convertedExpr = Expression.Convert(constant, typeof(object));
            return Expression.Call(convertedExpr, _toStringMethod);
        }
    }
}
