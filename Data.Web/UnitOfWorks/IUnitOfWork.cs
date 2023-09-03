using Cantin.Core.Entities;
using Cantin.Data.Repository.Abstract;

namespace Cantin.Data.UnitOfWorks
{ 
    public interface IUnitOfWork : IAsyncDisposable
    {
        IRepository<T> GetRepository<T>() where T : class, IEntityBase, new();
        Task<int> SaveAsync();
        int Save();
    }
}
