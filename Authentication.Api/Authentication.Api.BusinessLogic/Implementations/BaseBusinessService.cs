using Authentication.BusinessLogic.Interfaces;
using AuthenticationDataModels;
using DataAccessLayer.Interfaces;
using System.Linq.Expressions;

namespace Authentication.BusinessLogic.Implementations
{
    public class BaseBusinessService<T> : IBaseBusinessService<T> where T : class
    {
        protected readonly IGenericRepository<T, GlobalServDbContext> GenericRepository;
        public BaseBusinessService(IGenericRepository<T, GlobalServDbContext> generic)
        {
            GenericRepository = generic;
        }
        public virtual void Add(T entity)
        {
            GenericRepository.Add(entity);
            GenericRepository.Save();
        }

        public virtual void Delete(T entity)
        {
           GenericRepository.Delete(entity);
            GenericRepository.Save();
        }

        public IList<T> Get(Expression<Func<T, bool>> expression, bool asNoTracking = false,  params Expression<Func<T, object>>[] include)
        {
            return GenericRepository.Get(expression, asNoTracking, include);
        }

        public IList<T> GetAll()
        {
            return GenericRepository.GetAll();
        }

        public T? GetById<IdType>(IdType id)
        {
            return GenericRepository.GetById(id);
        }


        public void Update(T entity)
        {
            GenericRepository.Update(entity);
            GenericRepository.Save();
        }

    }
}
