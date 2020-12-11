using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ShadyNagy.Utilities.Api.DTOs;
using ShadyNagy.Utilities.Extensions.Types;

namespace ShadyNagy.Utilities.DesignPatterns.Specification
{

    public abstract class Specification<T, T2>
    {
        private static readonly Type _stringType = typeof(string);
        private static readonly Type _guidType = typeof(Guid);
        private static readonly Type _nullableGuidType = typeof(Guid?);

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
        public SortOrder SortOrder { get; set; }
        public object Value { get; set; }

        public T2 IsSatisfiedBy(T entity)
        {
            Func<T, T2> predicate = ToExpression().Compile();
            return predicate(entity);
        }


        public virtual Expression<Func<T, T2>> ToExpression()
        {
            var param = Expression.Parameter(typeof(T), "x");
            if (typeof(T2) == typeof(bool))
            {
                if (string.IsNullOrEmpty(PropertyName))
                {
                    return Expression.Lambda<Func<T, T2>>(Expression.Constant(true), param);
                }
                else
                {
                    var expression = GetFilter(param, PropertyName, FilterOperator, Value);
                    if (expression == null)
                    {
                        return Expression.Lambda<Func<T, T2>>(Expression.Constant(true), param);
                    }

                    return Expression.Lambda<Func<T, T2>>(expression, param);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(PropertyName))
                {
                    return Expression.Lambda<Func<T, T2>>(param, param);
                }
                else
                {
                    var expression = CreateSortExpression(param, PropertyName);
                    if (expression == null)
                    {
                        return Expression.Lambda<Func<T, T2>>(param, param);
                    }


                    return Expression.Lambda<Func<T, T2>>(expression, param);
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

        internal static Expression GetFilter(ParameterExpression parameter, string property, FilterOperator op, object value)
        {
            var constant = Expression.Constant(value is Guid ? value.ToString(): value);
            if (property.Contains("[") && property.Contains("]"))
            {
                var startArray = property.IndexOf("[", StringComparison.Ordinal);
                var finishArray = property.IndexOf("]", StringComparison.Ordinal);
                var baseName = property.Substring(0, startArray);
                var lastIndexOfDot = baseName.LastIndexOf(".", StringComparison.Ordinal);

                Expression paramNested;
                if (lastIndexOfDot > 0)
                {
                    paramNested = GetMember(parameter, baseName.Substring(0, lastIndexOfDot));
                    baseName = baseName.Substring(lastIndexOfDot + 1);
                }
                else
                {
                    paramNested = parameter;
                }

                if (paramNested == null)
                {
                    return null;
                }

                var name = property.Substring(startArray + 1, finishArray - startArray - 1);
                var type = paramNested.Type.GetRuntimePropertyWithoutCase(baseName).PropertyType.GenericTypeArguments[0];

                var methodAny = typeof(Enumerable).GetRuntimeMethods().First(x => x.Name == "Any" && x.GetParameters().Length == 2).MakeGenericMethod(type);
                var memberAny = GetMember(paramNested, baseName);
                if (memberAny == null)
                {
                    return null;
                }

                var parameterAny = Expression.Parameter(type, "i");
                var member = GetMember(parameterAny, name);
                if (member == null)
                {
                    return null;
                }

                var expressionAny = CreateFilter(member, op, constant);
                var expr2 = Expression.Lambda(expressionAny, parameterAny);

                return Expression.Call(methodAny, memberAny, expr2);
            }
            else
            {
                var member = GetMember(parameter, property);
                if (member == null)
                {
                    return null;
                }
                return CreateFilter(member, op, constant);
            }
        }

        internal static Expression CreateSortExpression(ParameterExpression parameter, string property)
        {
            if (property.Contains("[") && property.Contains("]"))
            {
                var startArray = property.IndexOf("[", StringComparison.Ordinal);
                var finishArray = property.IndexOf("]", StringComparison.Ordinal);
                var baseName = property.Substring(0, startArray);
                var lastIndexOfDot = baseName.LastIndexOf(".", StringComparison.Ordinal);

                Expression paramNested;
                if (lastIndexOfDot > 0)
                {
                    paramNested = GetMember(parameter, baseName.Substring(0, lastIndexOfDot));
                    baseName = baseName.Substring(lastIndexOfDot + 1);
                }
                else
                {
                    paramNested = parameter;
                }

                if (paramNested == null)
                {
                    return null;
                }

                var name = property.Substring(startArray + 1, finishArray - startArray - 1);
                var type = paramNested.Type.GetRuntimePropertyWithoutCase(baseName).PropertyType.GenericTypeArguments[0];

                var enumerableMethod = typeof(Enumerable).GetRuntimeMethods().First(x => x.Name == "FirstOrDefault" && x.GetParameters().Length == 1).MakeGenericMethod(type);
                var enumerableMember = GetMember(paramNested, baseName);
                if (enumerableMember == null)
                {
                    return null;
                }

                var parameterEnumerable = Expression.Call(enumerableMethod, enumerableMember);
                var member = GetMember(parameterEnumerable, name);

                return member;
            }
            else
            {
                var member = GetMember(parameter, property);

                return member;
            }
        }

        private static MemberExpression GetMember(Expression parameter, string propertyName)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));

            if (propertyName == null)
                throw new ArgumentNullException(nameof(propertyName));

            while (true)
            {
                if (propertyName.Contains("."))
                {
                    var index = propertyName.IndexOf(".", StringComparison.Ordinal);

                    var tempName = propertyName.Substring(0, index);
                    var propertyNameToUse = parameter.Type.GetRuntimePropertyNameWithoutCase(tempName);

                    var param = Expression.Property(parameter, propertyNameToUse);

                    parameter = param;
                    propertyName = propertyName.Substring(index + 1);

                    continue;
                }
                else
                {
                    propertyName = parameter.Type.GetRuntimePropertyNameWithoutCase(propertyName);
                }

                if (string.IsNullOrEmpty(propertyName))
                {
                    return null;
                }

                return Expression.Property(parameter, propertyName);
            }
        }

        private static Expression CreateFilter(MemberExpression prop, FilterOperator filterOperator, ConstantExpression constant)
        {
            switch (filterOperator)
            {
                case FilterOperator.Equals:
                    if (prop.Type == _guidType || prop.Type == _nullableGuidType)
                    {
                        var convertedToString = Expression.Call(prop, _toStringMethod);
                        return Expression.Equal(convertedToString,  constant);
                    }
                    else
                    {
                        return Expression.Equal(prop, PrepareConstantToSameType(prop, constant));
                    }
                        
                case FilterOperator.GreaterThan:
                    return Expression.GreaterThan(prop, PrepareConstantToSameType(prop, constant));
                case FilterOperator.LessThan:
                    return Expression.LessThan(prop, PrepareConstantToSameType(prop, constant));
                case FilterOperator.Contains:
                    return Expression.Call(prop, _containsMethod, PrepareConstant(constant));
                case FilterOperator.StartsWith:
                    return Expression.Call(prop, _startsWithMethod, PrepareConstant(constant));
                case FilterOperator.EndsWith:
                    return Expression.Call(prop, _endsWithMethod, PrepareConstant(constant));
                case FilterOperator.NotEqual:
                    return Expression.NotEqual(prop, PrepareConstantToSameType(prop, constant));
                case FilterOperator.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(prop, PrepareConstantToSameType(prop, constant));
                case FilterOperator.LessThanOrEqual:
                    return Expression.LessThanOrEqual(prop, PrepareConstantToSameType(prop, constant));
                    

                default:
                    throw new NotImplementedException();
            }
        }

        private static Expression PrepareConstant(ConstantExpression constant)
        {
            if (constant.Type == _stringType)
                return constant;

            return ToExpressionString(constant);
        }

        private static Expression PrepareConstantToSameType(MemberExpression prop, ConstantExpression constant)
        {
            if (constant.Type == prop.Type)
                return constant;

            return Expression.Convert(constant, prop.Type);
        }

        private static Expression ToExpressionString(Expression constant)
        {
            var convertedConstantObject = Expression.Convert(constant, typeof(object));
            return Expression.Call(convertedConstantObject, _toStringMethod);
        }
    }
}
