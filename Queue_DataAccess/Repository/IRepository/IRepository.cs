using Queue.Models;
using System.Linq.Expressions;

namespace Queue.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {

        T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false);

        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);

        void Add(T entity);
        void Remove(T entity);

        void Update(T entity);

        bool Any(Expression<Func<T, bool>> filter);


    }


}
