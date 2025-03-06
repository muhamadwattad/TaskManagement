namespace TaskManagement.DataAccessLayer.Repositories.Interfaces
{
    public interface IPaginate<T>
    {
        public int skip { get; set; }
        public int take { get; set; }
        public Func<IQueryable<T>, IOrderedQueryable<T>> GetOrdering(string? langKey = null);
    }
}
