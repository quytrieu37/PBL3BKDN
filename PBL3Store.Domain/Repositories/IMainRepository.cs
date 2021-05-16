using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBL3Store.Domain.Repositories
{
    public interface IMainRepository
    {
        IQueryable<Book> Books { get; }
        IQueryable<Category> Categories { get; }
        IQueryable<Payment> Payments { get; }
        IQueryable<User> Users { get; }
        IQueryable<Role> Roles { get; }
        IQueryable<Order> order { get; }
        IQueryable<OrderDetail> OrderDetails { get; }
        void Add(User user);
        void Add(Order order);
        void Add(OrderDetail orderDetail);
        void Add(Book book);
        void Edit(Book book);
        void Remove(Book book);
        void Edit(User User);
    }
}
