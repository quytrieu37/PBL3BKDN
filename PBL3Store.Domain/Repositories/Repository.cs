using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBL3Store.Domain.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly PBL3Entities _context;
        private DbSet<T> List;
        public Repository()
        {
            _context = new PBL3Entities();
            List = _context.Set<T>();
        }
        public void Add(T obj)
        {
            List.Add(obj);
        }

        public void Delete(T obj)
        {
            _context.Entry(obj).State = EntityState.Deleted;
        }

        public void DeleteById(int Id)
        {
            T exist = List.Find(Id);
            List.Remove(exist);
        }

        public void Edit(T obj)
        {
            _context.Entry(obj).State = EntityState.Modified;
        }

        public IEnumerable<T> GetALl()
        {
            return List;
        }
        public void SaveChange()
        {
            _context.SaveChanges();
        }
    }
}
