using System.Data;

namespace CoffeeShop.UnitOfWork
{
    public interface IUnitOfWork
    {
        IDbConnection Connection { get; }
        Task BeginTransactionAsync(IsolationLevel level = IsolationLevel.ReadCommitted);
        Task CommitAsync();
        Task RollbackAsync();
    }
}