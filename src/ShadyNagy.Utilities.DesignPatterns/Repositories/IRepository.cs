using System;
using System.Collections.Generic;
using System.Text;
using ShadyNagy.Utilities.Api.DTOs;
using ShadyNagy.Utilities.DesignPatterns.Specification;
using Microsoft.EntityFrameworkCore;

namespace ShadyNagy.Utilities.DesignPatterns.Repositories
{
    public interface IRepository<TModel>
    {
        IReadOnlyList<TModel> GetAllByFilter(Specification<TModel, bool> specification,
            Specification<TModel, object> orderSpecification = null, SortOrder? sortOrder = null);
        IReadOnlyList<TModel> GetAllByFilterAndPage(Specification<TModel, bool> specification,
            Specification<TModel, object> orderSpecification = null, SortOrder? sortOrder = null, int? pageNumber = 0);
        TModel GetById(string id);
        TModel Add(TModel entity);
        TModel Update(TModel entity, TModel oldEntity);
        bool Delete(string id);
        bool Delete(IReadOnlyList<string> ids);
        void SaveChanges();
        string GetConnectionString();
        DbContext GetDbContext();
        int GetCount(Specification<TModel, bool> specification);
        void SetUserId(Guid? userId);
        void Reset();
    }
}
