﻿using System.Collections.Generic;

namespace ShadyNagy.Utilities.DesignPatterns.OrderBySpecification
{
    public sealed class Configuration<TEntity> : IOrderedSpecification<TEntity>
        where TEntity : class
    {
        public Configuration(IQuerySpecification<TEntity> querySpecification)
        {
            OrderSpecifications = new List<IOrderSpecification<TEntity>>();
            QuerySpecification = querySpecification;
        }

        public Configuration(IOrderSpecification<TEntity> orderSpecification)
        {
            OrderSpecifications = new List<IOrderSpecification<TEntity>> { orderSpecification };
            QuerySpecification = new DummySpecification<TEntity>();
        }

        public Configuration(IQuerySpecification<TEntity> querySpecification, List<IOrderSpecification<TEntity>> orderSpecifications)
        {
            OrderSpecifications = orderSpecifications;
            QuerySpecification = querySpecification;
        }

        public IQuerySpecification<TEntity> QuerySpecification { get; set; }

        public List<IOrderSpecification<TEntity>> OrderSpecifications { get; }

        public int? Skip { get; set; }

        public int? Take { get; set; }

        public Configuration<TEntity> Internal => this;

        public ISpecification<TEntity> Clone()
        {
            return new Configuration<TEntity>(QuerySpecification, OrderSpecifications)
            {
                Skip = Skip,
                Take = Take
            };
        }
    }
}
