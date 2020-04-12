using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services
{
    public interface IRepository<T> : IReadRepository<T>, IDisposable where T : class
    {
        IEnumerable<T> GetAll();
        void Add(T entity);
        void Add(params T[] entities);
        void Add(IEnumerable<T> entities);


        void Delete(T entity);
        Task<bool> Delete(object id);
        void Delete(params T[] entities);
        void Delete(IEnumerable<T> entities);
        T GetByID(int id);
        T GetByName(string name);
        void Update(T entity);
        void Update(params T[] entities);
        void Update(IEnumerable<T> entities);
    }
}
