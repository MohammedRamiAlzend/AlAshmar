using AlAshmar.Application.Repos;
using AlAshmar.Domain.Entities.Abstraction;

namespace AlAshmar.Infrastructure.Persistence.UnitOfWork;




public interface IUnitOfWork : IDisposable
{



    IRepositoryBase<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : Entity<TKey>;




    Task<int> SaveChangesAsync();




    Task BeginTransactionAsync();




    Task CommitTransactionAsync();




    Task RollbackTransactionAsync();
}
