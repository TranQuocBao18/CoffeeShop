using CoffeeShop.Domain.Shared.Common;

namespace CoffeeShop.Infrastructure.Shared.Interfaces
{
    public interface ISearchRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        IQueryable<TEntity> AsQueryable();
    }
}