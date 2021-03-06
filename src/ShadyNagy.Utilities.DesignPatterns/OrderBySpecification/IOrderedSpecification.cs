﻿namespace ShadyNagy.Utilities.DesignPatterns.OrderBySpecification
{ 
    // ReSharper disable once UnusedMember.Global
    public interface IOrderedSpecification<TEntity> : ISpecification<TEntity>
        where TEntity : class
    {
    }
}
