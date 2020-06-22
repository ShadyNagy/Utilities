using System.Collections.Generic;
using ShadyNagy.Utilities.Api;
using ShadyNagy.Utilities.Api.DTOs;

namespace ShadyNagy.Utilities.DesignPatterns.Services.Interfaces
{
    public interface IService<TDto>
    {
        void ResetRepository();
        void SetUserId(string userId);
        Result<IReadOnlyList<TDto>, int> GetAll();
        Result<IReadOnlyList<TDto>, int> GetWithFilters(LookupGetRequest lookupGetRequest);
        Result<IReadOnlyList<TSelectDto>, int> GetSelectList<TSelectDto>();
        Result<IReadOnlyList<TDto>, int> GetAllActive();
        Result<TDto, int> GetById(string id);
        Result<TDto, int> Add(TDto deviceType);
        Result<TDto, int> Update(string id, TDto deviceType);
        Result<TDto, int> Delete(string id);
        Result<TDto, int> Delete(List<string> ids);
        Result<bool, int> CheckIsUnique(string fieldName, string value, string id = null);

    }
}
