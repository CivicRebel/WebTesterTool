using WebTester.Repositories.Entities;
using WebTester.Repositories.Interfaces;

namespace WebTester.Repositories.Base
{
    public abstract class BaseRepository<T, TId> : IRepository<T,TId> where T : class, IEntity<TId>
    {
        protected readonly IEnumerable<T> _entities;
        
        public BaseRepository(IEnumerable<T> entities) { 
            _entities = entities;
        }

        public IEnumerable<T> GetAll()
        {
            return _entities;
        }

        public T? GetById(TId id)
        {
            return _entities.SingleOrDefault(x => x.Id.Equals(id));
        }
    }
}
