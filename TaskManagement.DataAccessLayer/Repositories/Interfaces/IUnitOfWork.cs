using TaskManagement.DataAccessLayer.Entities;

namespace TaskManagement.DataAccessLayer.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        void Clear();
        Task<int> Commit(CancellationToken? cancellationToken = null);
        IRepository<T> Repository<T>() where T : Entity;
    }
}
