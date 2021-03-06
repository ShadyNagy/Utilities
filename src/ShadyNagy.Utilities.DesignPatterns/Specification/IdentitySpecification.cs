﻿using System;
using System.Linq.Expressions;

namespace ShadyNagy.Utilities.DesignPatterns.Specification
{
    internal sealed class IdentitySpecification<T, T2> : Specification<T, T2>
    {
        public override Expression<Func<T, T2>> ToExpression()
        {
            if (typeof(T2) == typeof(bool))
            {
                return x => (T2)(object)Convert.ToBoolean(true);
            }

            return x => default;
        }
    }
}
