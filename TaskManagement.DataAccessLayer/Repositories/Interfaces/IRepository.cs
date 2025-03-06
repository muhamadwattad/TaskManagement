using System.Linq.Expressions;
using TaskManagement.DataAccessLayer.Entities;

namespace TaskManagement.DataAccessLayer.Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        IFilterBuilder<TEntity> GetFilterBuilder { get; }



        Task<List<OUT>> GetAsync<OUT>(Expression<Func<TEntity, OUT>> select, IEnumerable<Expression<Func<TEntity, bool>>>? filters = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, int skip = 0, int take = -1, params string[] populate);
        Task<List<TEntity>> GetAsyncWithoutSelect(IEnumerable<Expression<Func<TEntity, bool>>>? filters = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, int skip = 0, int take = -1, params string[] populate);
        Task<TEntity> GetByIdAsync(Guid id, params string[] includes);
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task<bool> Exists(Expression<Func<TEntity, bool>> filter);
    }
}
