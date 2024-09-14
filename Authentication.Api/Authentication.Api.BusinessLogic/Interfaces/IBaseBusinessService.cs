using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.BusinessLogic.Interfaces
{
    public interface IBaseBusinessService<T> where T : class
    {
        IList<T> GetAll();
        T? GetById<IdType>(IdType id);
        void Delete(T entity);
        void Add(T entity);
        void Update(T entity);
        IList<T> Get(Expression<Func<T, bool>> expression, bool asNoTracking = false, params Expression<Func<T, object>>[] include);
    }
}
