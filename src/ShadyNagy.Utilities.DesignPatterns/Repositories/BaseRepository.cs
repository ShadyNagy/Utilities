using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if NETSTANDARD2_0
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Proxies;
#elif NET
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Proxies;
#endif
using ShadyNagy.Utilities.Api.DTOs;
using ShadyNagy.Utilities.DesignPatterns.Specification;
using ShadyNagy.Utilities.Extensions;
using ShadyNagy.Utilities.Extensions.Object;

namespace ShadyNagy.Utilities.DesignPatterns.Repositories
{
    public abstract class BaseRepository<TModel>: IRepository<TModel> 
        where TModel : class
    {
        #region Fields
#if NETFRAMEWORK
        protected System.Data.Entity.DbContext DbContext { get; set; }
#elif NET
        protected Microsoft.EntityFrameworkCore.DbContext DbContext { get; set; }
#else
        protected Microsoft.EntityFrameworkCore.DbContext DbContext { get; set; }
#endif
        private string ConnectionString { get; set; }
        private int PageSize { get; set; }
        private Guid? UserId { get; set; }

#endregion

#region Constructors

        protected BaseRepository(string connectionString, Guid? userId = null, int pageSize = 25)
        {
            ConnectionString = connectionString;
            PageSize = pageSize;
            UserId = userId;
        }

#endregion

#region Actions

        public string GetConnectionString()
        {
            return ConnectionString;
        }
#if NETFRAMEWORK
        public System.Data.Entity.DbContext GetDbContext()
        {
            return DbContext;
        }
#elif NET
         public Microsoft.EntityFrameworkCore.DbContext GetDbContext()
        {
            return DbContext;
        }
#else
        public Microsoft.EntityFrameworkCore.DbContext GetDbContext()
        {
            return DbContext;
        }
#endif
        public int GetCount(Specification<TModel, bool> specification)
        {
      
            return DbContext.Set<TModel>()
                .Count(specification?.ToExpression() ?? throw new InvalidOperationException());
        }

        public void SetUserId(Guid? userId)
        {
            UserId = userId;
        }

        public void Reset()
        {
            DbContext.Reset();
        }

        public IReadOnlyList<TModel> GetAll()
        {
            return DbContext.Set<TModel>()?
                .OrderBy("CreatedDate")
                .ToList() as List<TModel>;
        }

        public IReadOnlyList<TModel> GetAllByFilter(Specification<TModel, bool> specification, Specification<TModel, object> orderSpecification = null, SortOrder? sortOrder = null)
        {
            if (orderSpecification == null || sortOrder == null)
            {
                return DbContext.Set<TModel>()?
                    .Where(specification?.ToExpression() ?? throw new InvalidOperationException())?
                    .OrderBy("CreatedDate")
                    .ToList() as List<TModel>;
            }
            else
            {
                return DbContext.Set<TModel>()
                    .Where(specification?.ToExpression() ?? throw new InvalidOperationException())
                    .OrderBy(orderSpecification?.ToExpression() ?? throw new InvalidOperationException())
                    .ToList() as List<TModel>;
            }
        }

        public IReadOnlyList<TModel> GetAllByFilterAndPage(Specification<TModel, bool> specification, Specification<TModel, object> orderSpecification = null, SortOrder? sortOrder = null, int? pageNumber = 0)
        {
            if (orderSpecification == null || sortOrder == null)
            {
                return DbContext.Set<TModel>()
                    .Where(specification?.ToExpression() ?? throw new InvalidOperationException())
                    .OrderBy("CreatedDate")
                    .Skip(PageSize * pageNumber ?? 0)
                    .Take(PageSize)?
                    .ToList() as List<TModel>;
            }
            else
            {
                if (sortOrder == SortOrder.Asc)
                {
                    return DbContext.Set<TModel>()
                        .Where(specification?.ToExpression() ?? throw new InvalidOperationException())
                        .OrderBy(orderSpecification?.ToExpression() ?? throw new InvalidOperationException())
                        .Skip(PageSize * pageNumber ?? 0)
                        .Take(PageSize)?
                        .ToList() as List<TModel>;
                }
                else
                {
                    return DbContext.Set<TModel>()
                        .Where(specification?.ToExpression() ?? throw new InvalidOperationException())
                        .OrderByDescending(orderSpecification?.ToExpression() ?? throw new InvalidOperationException())
                        .Skip(PageSize * pageNumber ?? 0)
                        .Take(PageSize)?
                        .ToList() as List<TModel>;
                }

            }
        }
        public void UpdateWithoutTrack<TTModel>(object entity)
            where TTModel : class
        {
            var propertyName = GetPrimaryKeyName<TTModel>();
            var id = GetValueByPropertyName<TTModel>(entity);

            //DbContext.Entry(DbContext.Set<TTModel>().FirstOrDefaultPropertyName(propertyName, id) ?? throw new InvalidOperationException()).Reload();
            DbContext.Entry(DbContext.Set<TTModel>().FirstOrDefaultPropertyName(propertyName, id) ?? throw new InvalidOperationException()).CurrentValues.SetValues(entity);
        }

        public virtual TModel GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }

            var primaryType = GetPrimaryKeyType<TModel>();
            if (typeof(Guid) == primaryType)
            {
                return DbContext.Set<TModel>().Find(new Guid(id));
            }else if (typeof(string) == primaryType)
            {
                return DbContext.Set<TModel>().Find(id);
            }else if (typeof(int) == primaryType)
            {
                return DbContext.Set<TModel>().Find(int.Parse(id));
            }

            return null;
        }

        public virtual TModel GetByIdWithoutCache(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }

            DbContext.Reset();



            var primaryType = GetPrimaryKeyType<TModel>();
            var primaryKeyName = GetPrimaryKeyName<TModel>();

            if (typeof(Guid) == primaryType)
            {
                var entity = DbContext.Set<TModel>().FirstOrDefaultPropertyName(primaryKeyName, new Guid(id));
                return entity;
            }
            else if (typeof(string) == primaryType)
            {
                var entity = DbContext.Set<TModel>().FirstOrDefaultPropertyName(primaryKeyName, id);

                return entity;
            }
            else if (typeof(int) == primaryType)
            {
                var entity = DbContext.Set<TModel>().FirstOrDefaultPropertyName(primaryKeyName, int.Parse(id));

                return entity;
            }

            return null;
        }

        public virtual TModel Add(TModel entity)
        {
            if (entity == null)
            {
                return null;
            }
            if (UserId != null && entity.GetType().GetProperty("CreatedBy") != null)
            {
                entity.GetType().GetProperty("CreatedBy")?.SetValue(entity, (Guid)UserId);
            }

            if (entity.GetType().GetProperty("CreatedDate") != null)
            {
                entity.GetType().GetProperty("CreatedDate")?.SetValue(entity, DateTime.Now);
            }

            if (entity.GetType().GetProperty("IsSystem") != null)
            {
                entity.GetType().GetProperty("IsSystem")?.SetValue(entity, false);
            }

#if NETFRAMEWORK
            return DbContext
                .Set<TModel>()
                .Add(entity);
#elif NET
        return DbContext
                .Add(entity)
                .Entity;
#else
            //var toCreate = DbContext.CreateProxy<TModel>();
            //DbContext.Entry(toCreate).CurrentValues.SetValues(entity);
            //return DbContext.Add(toCreate).Entity;
            return DbContext
                .Add(entity)
                .Entity;
#endif
        }

        public virtual TModel Update(TModel entity, TModel oldEntity)
        {
            if (entity == null)
            {
                return null;
            }

            if (UserId != null && entity.GetType().GetProperty("CreatedBy") != null)
            {
                entity.GetType().GetProperty("CreatedBy")?.SetValue(entity,
                    oldEntity.GetType().GetProperty("CreatedBy")?.GetValue(oldEntity));
            }

            if (entity.GetType().GetProperty("CreatedDate") != null)
            {
                entity.GetType().GetProperty("CreatedDate")?.SetValue(entity,
                    oldEntity.GetType().GetProperty("CreatedDate")?.GetValue(oldEntity));
            }

            if (UserId != null && entity.GetType().GetProperty("ModifiedBy") != null)
            {
                entity.GetType().GetProperty("ModifiedBy")?.SetValue(entity, (Guid)UserId);
            }

            if (entity.GetType().GetProperty("ModifiedDate") != null)
            {
                entity.GetType().GetProperty("ModifiedDate")?.SetValue(entity, DateTime.Now);
            }

            if (entity.GetType().GetProperty("IsSystem") != null)
            {
                entity.GetType().GetProperty("IsSystem")?.SetValue(entity, false);
            }

            var oldTranslations = GetTranslations(oldEntity);
            var newTranslationsList = GetTranslations(entity);
            var translationIdPropertyName = GetTranslationIdPropertyName(oldEntity);


            if (oldTranslations != null)
            {

                foreach (var translation in oldTranslations)
                {
                    if (newTranslationsList == null)
                    {
                        continue;
                    }

                    if (!HasTranslation(newTranslationsList, translation, translationIdPropertyName))
                    {
#if NETFRAMEWORK
                        DbContext.Entry(translation).State = System.Data.Entity.EntityState.Deleted;
#elif NET
                        DbContext.Remove(translation);
#else
                        DbContext.Remove(translation);
#endif
                    }
                }

                foreach (var translation in newTranslationsList)
                {
                    if (IsNew(translation, translationIdPropertyName))
                    {
#if NETFRAMEWORK
                        DbContext.Entry(translation).State = System.Data.Entity.EntityState.Added;
#elif NET
                        DbContext.Add(translation);
#else
                        DbContext.Add(translation);
#endif
                    }
                    else
                    {
                        UpdateEntity(oldTranslations, translation, translationIdPropertyName);
                    }
                }

            }

            UpdateWithoutTrack<TModel>(entity);

            return entity;
        }

        public virtual bool Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return false;
            }

            var entity = GetById(id);
            if (entity == null)
            {
                return false;
            }

            if (entity.GetType().GetProperty("IsSystem") != null)
            {
                var isSystem = entity.GetType().GetProperty("IsSystem")?.GetValue(entity);
                if (isSystem != null && (bool)isSystem)
                {
                    return false;
                }
            }

            var translations = GetTranslations(entity);
            if (translations != null)
            {
                foreach (var translation in translations)
                {
#if NETFRAMEWORK
                    DbContext.Entry(translation).State = System.Data.Entity.EntityState.Deleted;
#elif NET
                    DbContext.Remove(translation);
#else
                    DbContext.Remove(translation);
#endif
                }
            }

            DbContext.Set<TModel>().Remove(entity);

            return true;
        }

        private IEnumerable GetTranslations(TModel entity)
        {
            if (entity == null)
            {
                return null;
            }
            var translationProperty = entity.GetType().GetProperties()
                .FirstOrDefault(x => x.Name.ToLower().Contains("translation"));
            if (translationProperty != null)
            {

                if (translationProperty.GetValue(entity) is IEnumerable translationData)
                {
                    var propertyName = translationProperty.Name;
                    var translationIdPropertyName = $"{propertyName}Id";
                    var newTranslationsList = entity.GetPropertyValue(propertyName) as IEnumerable;
                    return newTranslationsList;
                }
            }

            return null;
        }

        private string GetTranslationIdPropertyName(TModel entity)
        {
            if (entity == null)
            {
                return null;
            }

            var translationProperty = entity.GetType().GetProperties()
                .FirstOrDefault(x => x.Name.ToLower().Contains("translation"));
            if (translationProperty != null)
            {

                if (translationProperty.GetValue(entity) is IEnumerable translationData)
                {
                    var propertyName = translationProperty.Name;
                    return $"{propertyName}Id";
                }
            }

            return null;
        }

        public virtual bool Delete(IReadOnlyList<string> ids)
        {
            var res = true;

            for (var i = 0; i < ids.Count; i++)
            {
                var isExist = Delete(ids[i]);
                if (res)
                {
                    res = isExist;
                }
            }

            return res;
        }
        public void SaveChanges()
        {
            DbContext.SaveChanges();
        }

        private Type GetPrimaryKeyType<TTModel>() 
            where TTModel : class
        {
#if NETFRAMEWORK
            return DbContext.GetKeyTypes<TTModel>().Single();
#elif NET
            return DbContext.Model.FindEntityType(typeof(TTModel)).FindPrimaryKey().Properties
                .Select(x => x.ClrType).Single();
#else
            return DbContext.Model.FindEntityType(typeof(TTModel)).FindPrimaryKey().Properties
                .Select(x => x.ClrType).Single();
#endif
        }

#if NETFRAMEWORK
        private string GetPrimaryKeyName<TTModel>()
            where TTModel : class
        {
            return DbContext.GetKeyNames<TTModel>().Single();
        }
#elif NET
        private string GetPrimaryKeyName<TTModel>()
            where TTModel : class, IReadOnlyModel
        {
            return DbContext.Model.FindEntityType(typeof(TTModel)).FindPrimaryKey().Properties
                .Select(x => x.Name).Single();
        }
#else
        private string GetPrimaryKeyName<TTModel>()
            where TTModel : class
        {
            return DbContext.Model.FindEntityType(typeof(TTModel)).FindPrimaryKey().Properties
                .Select(x => x.Name).Single();
        }
#endif

        private object GetValueByPropertyName<TTModel>(object entity)
            where TTModel : class
        {
            var keyName = GetPrimaryKeyName<TTModel>();

            return entity.GetType().GetProperty(keyName)?.GetValue(entity, null);
        }

        private bool IsNew(object translation, string translationIdPropertyName)
        {
            return (translation.GetPropertyValue(translationIdPropertyName) == null) ||
                   (translation.GetPropertyType(translationIdPropertyName) == typeof(int) && translation.GetPropertyValue(translationIdPropertyName).ToString() == 0.ToString()) ||
                   (translation.GetPropertyType(translationIdPropertyName) == typeof(string) && string.IsNullOrEmpty(translation.GetPropertyValue(translationIdPropertyName).ToString())) ||
                   (translation.GetPropertyType(translationIdPropertyName) == typeof(Guid) && translation.GetPropertyValue(translationIdPropertyName).ToString() == Guid.Empty.ToString());
        }

        private void UpdateEntity(IEnumerable translationsSearchIn, object translation, string translationIdPropertyName)
        {
            var oldTranslationEntity = GetEntity(translationsSearchIn, translation, translationIdPropertyName);
            DbContext.Entry(oldTranslationEntity).CurrentValues.SetValues(translation);
        }

        private object GetEntity(IEnumerable translationsSearchIn, object translation, string translationIdPropertyName)
        {
            foreach (var translationSearchIn in translationsSearchIn)
            {
                var translationSearchInId = translationSearchIn.GetPropertyValue(translationIdPropertyName);
                var translationId = translation.GetPropertyValue(translationIdPropertyName);
                if (translationId.ToString().ToLower() == translationSearchInId.ToString().ToLower())
                {
                    return translationSearchIn;
                }
            }

            return null;
        }

        private bool HasTranslation(IEnumerable translationsSearchIn, object translation, string translationIdPropertyName)
        {
            foreach (var translationSearchIn in translationsSearchIn)
            {
                var translationSearchInId = translationSearchIn.GetPropertyValue(translationIdPropertyName);
                var translationId = translation.GetPropertyValue(translationIdPropertyName);
                if (translationId.ToString().ToLower() == translationSearchInId.ToString().ToLower())
                {
                    return true;
                }
            }

            return false;
        }


#endregion

    }
}

