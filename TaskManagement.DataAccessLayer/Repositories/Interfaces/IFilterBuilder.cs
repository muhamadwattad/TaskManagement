using System.Linq.Expressions;

namespace TaskManagement.DataAccessLayer.Repositories.Interfaces
{
    public interface IFilterBuilder<T>
    {
        IFilterBuilder<T> Add(Expression<Func<T, bool>> filter);
        IEnumerable<Expression<Func<T, bool>>> Build();
    }
}
