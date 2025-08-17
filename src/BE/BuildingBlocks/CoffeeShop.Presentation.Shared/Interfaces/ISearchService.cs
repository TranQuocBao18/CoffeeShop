using CoffeeShop.Domain.Shared.Common;
using CoffeeShop.Infrastructure.Shared.Wrapper;
using CoffeeShop.Model.Dto.Shared.Filters;

namespace CoffeeShop.Presentation.Shared.Interfaces
{
    public interface ISearchService<TEntity, TKey, TCriteria, TResponse> where TCriteria : BaseCriteria
                                                                         where TEntity : class, IEntity<TKey>
                                                                         where TResponse : class, new()
    {
        Task<BaseSearchResponse<TResponse>> SearchAsync(TCriteria criteria);
    }
}