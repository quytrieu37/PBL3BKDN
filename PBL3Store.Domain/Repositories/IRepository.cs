using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBL3Store.Domain.Repositories
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetALl();
        void Add(T obj);
        void Delete(T obj);
        void DeleteById(int Id);
        void Edit(T obj);
        void SaveChange();
    }
}
