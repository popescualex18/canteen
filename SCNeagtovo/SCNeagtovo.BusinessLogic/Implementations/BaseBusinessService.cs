using SCNeagtovo.BusinessLogic.Interfaces;
using SCNeagtovo.DataAccessLayer.Interfaces;
using System.Linq.Expressions;

namespace SCNeagtovo.BusinessLogic.Implementations
{
    public class BaseBusinessService<T> : IBaseBusinessService<T> where T : class
    {
        private readonly IGenericRepository<T> _genericRepository;
        public BaseBusinessService(IGenericRepository<T> generic)
        {
            _genericRepository = generic;
        }
        public void Add(T entity)
        {
            _genericRepository.Add(entity);
            _genericRepository.Save();
        }

        public void Delete(T entity)
        {
            _genericRepository.Delete(entity);
            _genericRepository.Save();
        }

        public IList<T> Get(Expression<Func<T, bool>> expression, bool asNoTracking = false, params Expression<Func<T, object>>[] include)
        {
            return _genericRepository.Get(expression, asNoTracking, include);
        }

        public IList<T> GetAll()
        {
            return _genericRepository.GetAll();
        }

        public IQueryable<T> GetAllAsQueryable()
        {
            return _genericRepository.GetAllAsQueryable();
        }

        public T? GetById<IdType>(IdType id)
        {
            return _genericRepository.GetById(id);
        }


        public void Update(T entity, IList<string>? ignoreProperties = null)
        {
            _genericRepository.Update(entity, ignoreProperties ?? new List<string>());
            _genericRepository.Save();
        }

        public void UpdateProperty(string tableName, string entityId, string property, string propetyValue)
        {
            _genericRepository.UpdateProperty(tableName, entityId, property, propetyValue);
            _genericRepository.Save();
        }
    }
}
