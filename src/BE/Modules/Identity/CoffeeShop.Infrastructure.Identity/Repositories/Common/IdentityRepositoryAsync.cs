using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using CoffeeShop.Domain.Shared.Common;
using CoffeeShop.Infrastructure.Identity.Contexts;
using CoffeeShop.Infrastructure.Shared.Interfaces;

namespace CoffeeShop.Infrastructure.Identity.Repositories
{
    public class IdentityRepositoryAsync<T> : IGenericRepository<T, Guid> where T : BaseEntityWithAudit
    {
        private readonly IdentityContext _dbContext;

        public IdentityRepositoryAsync(IdentityContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region Read Methods

        public IQueryable<T> AsQueryable()
        {
            throw new NotImplementedException();
        }

        public virtual async Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken, bool includedDeleted = false)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> GetPagedReponseAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            return await _dbContext
                .Set<T>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbContext
                 .Set<T>()
                 .ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _dbContext
                 .Set<T>()
                 .Where(predicate)
                 .ToListAsync();
        }

        public async virtual Task<bool> AllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _dbContext.Set<T>().AllAsync(predicate, cancellationToken);
        }

        public async virtual Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _dbContext.Set<T>().AnyAsync(predicate, cancellationToken);
        }

        public async virtual Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _dbContext.Set<T>().CountAsync(predicate, cancellationToken);
        }

        public async virtual Task<TResult> MaxAsync<TResult>(Expression<Func<T, TResult>> predicate, CancellationToken cancellationToken)
        {
            return await _dbContext.Set<T>().MaxAsync(predicate, cancellationToken);
        }

        public async virtual Task<TResult> MinAsync<TResult>(Expression<Func<T, TResult>> predicate, CancellationToken cancellationToken)
        {
            return await _dbContext.Set<T>().MinAsync(predicate, cancellationToken);
        }

        #endregion

        #region Write Methods

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken, bool hasTransaction = false)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            if (!hasTransaction)
            {
                await _dbContext.SaveChangesAsync();
            }
            return entity;
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken, bool hasTransaction = false)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            if (!hasTransaction)
            {
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(T entity, CancellationToken cancellationToken, bool hasTransaction = false)
        {
            _dbContext.Set<T>().Remove(entity);
            if (!hasTransaction)
            {
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task SoftDeleteAsync(T entity, CancellationToken cancellationToken, bool hasTransaction = false)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            if (!hasTransaction)
            {
                await _dbContext.SaveChangesAsync();
            }
        }

        public Task<bool> AddRangeAsync(IList<T> entities, CancellationToken cancellationToken, bool hasTransaction = false)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRangeAsync(IList<T> entities, CancellationToken cancellationToken, bool hasTransaction = false)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRangeAsync(IList<T> entities, CancellationToken cancellationToken, bool hasTransaction = false)
        {
            throw new NotImplementedException();
        }

        public Task SoftDeleteRangeAsync(IList<T> entities, CancellationToken cancellationToken, bool hasTransaction = false)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
