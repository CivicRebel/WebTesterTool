using WebTester.Repositories.Entities;

namespace WebTester.Repositories.Interfaces
{
    public interface IRepository<T, TId> where T : class, IEntity<TId>
    {
        public IEnumerable<T> GetAll();
        public T? GetById(TId id);
    }
}
