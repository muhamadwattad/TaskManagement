using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.DataAccessLayer.Entities;
using TaskManagement.DataAccessLayer.Repositories.Interfaces;

namespace TaskManagement.DataAccessLayer.Db
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private readonly CancellationToken _cancellationToken;
        private Hashtable _repositories = [];


        public UnitOfWork(AppDbContext dbContext, CancellationToken cancellationToken)
        {
            _dbContext = dbContext;
            _cancellationToken = cancellationToken;
        }

        public void Clear() => _dbContext.ChangeTracker.Clear();

        public async Task<int> Commit(CancellationToken? cancellationToken = null)
        {
            foreach (var entry in _dbContext.ChangeTracker.Entries<Entity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        break;
                }
            }

            return await _dbContext.SaveChangesAsync(cancellationToken ?? _cancellationToken);
        }

        public IRepository<T> Repository<T>() where T : Entity
        {
            var type = typeof(T).Name;

            if (_repositories.ContainsKey(type)) return (IRepository<T>)_repositories[type]!;

            var repositoryType = typeof(DataAccessLayer.Repositories.Repository<>);
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _dbContext, _cancellationToken);

            _repositories.Add(type, repositoryInstance);

            return (IRepository<T>)_repositories[type]!;
        }

        public TRepository CustomRepository<TRepository>() where TRepository : IRepository<Entity>
        {
            var type = typeof(TRepository).Name;

            if (_repositories.ContainsKey(type)) return (TRepository)_repositories[type]!;

            var repositoryType = typeof(DataAccessLayer.Repositories.Repository<>);
            var repositoryInstance =
                Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TRepository)), _dbContext);

            _repositories.Add(type, repositoryInstance);

            return (TRepository)_repositories[type]!;
        }
    }
}
