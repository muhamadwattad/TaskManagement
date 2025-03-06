using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManagement.DataAccessLayer.Entities;
using TaskManagement.DataAccessLayer.Repositories.Interfaces;
using TaskManagement.Framework.Exceptions;

namespace TaskManagement.DataAccessLayer.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;
        private readonly CancellationToken _cancellationToken;

        public Repository(DbContext dbContext, CancellationToken cancellationToken)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
            _cancellationToken = cancellationToken;
        }

        public class FilterBuilder : IFilterBuilder<TEntity>
        {
            private readonly List<Expression<Func<TEntity, bool>>> filters;
            public FilterBuilder()
            {
                filters = new List<Expression<Func<TEntity, bool>>>();
            }

            public IFilterBuilder<TEntity> Add(Expression<Func<TEntity, bool>> filter)
            {
                filters.Add(filter);
                return this;
            }

            public IEnumerable<Expression<Func<TEntity, bool>>> Build()
            {
                return filters;
            }
        }

        public IFilterBuilder<TEntity> GetFilterBuilder => new FilterBuilder();


        public async Task<List<OUT>> GetAsync<OUT>(
            Expression<Func<TEntity, OUT>> select,
            IEnumerable<Expression<Func<TEntity, bool>>>? filters = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            int skip = 0,
            int take = -1,
            params string[] populate)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();

            if (filters != null)
                foreach (var f in filters)
                    query = query.Where(f);

            foreach (var includeProperty in populate)
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
                query = orderBy(query);

            query = query.Skip(skip).Take(take == -1 ? int.MaxValue : take);

            return await query.Select(select).ToListAsync(_cancellationToken);
        }


        public async Task<TEntity> GetByIdAsync(Guid id, params string[] includes)
        {
            IQueryable<TEntity> query = _dbSet.AsQueryable();

            foreach (string includeProperty in includes)
            {
                query = query.Include(includeProperty);
            }

            return await query.FirstOrDefaultAsync(s => s.Id == id, _cancellationToken) ?? throw new NotFoundException(nameof(TEntity) + " Not Found");
        }

        public Task AddAsync(TEntity entity)
        {
            _dbSet.Add(entity);
            return Task.CompletedTask;
        }
        public async Task UpdateAsync(TEntity entity)
        {
            _dbContext.Update(entity);
        }

        public async Task<bool> Exists(Expression<Func<TEntity, bool>> filter)
        {
            IQueryable<TEntity> query = _dbSet;
            return await query.AnyAsync(filter, _cancellationToken);
        }
        public async Task<List<TEntity>> GetAsyncWithoutSelect(IEnumerable<Expression<Func<TEntity, bool>>>? filters = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, int skip = 0, int take = -1, params string[] populate)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();

            if (filters != null)
                foreach (var f in filters)
                    query = query.Where(f);

            foreach (var includeProperty in populate)
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
                query = orderBy(query);

            query = query.Skip(skip).Take(take == -1 ? int.MaxValue : take);

            return await query.ToListAsync(_cancellationToken);
        }

    }
}
