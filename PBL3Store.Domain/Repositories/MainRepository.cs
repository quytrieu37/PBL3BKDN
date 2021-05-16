using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBL3Store.Domain.Repositories
{
    public class MainRepository : IMainRepository
    {
        private readonly PBL3Entities _context;
        public MainRepository()
        {
            _context = new PBL3Entities();
        }
        public IQueryable<Book> Books => _context.Books;

        public IQueryable<Category> Categories => _context.Categories;

        public IQueryable<Payment> Payments => _context.Payments;

        public IQueryable<User> Users => _context.Users;

        public IQueryable<Role> Roles => _context.Roles;

        public IQueryable<Order> order => _context.Orders;

        public IQueryable<OrderDetail> OrderDetails => _context.OrderDetails;

        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Add(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public void Add(OrderDetail orderDetail)
        {
            _context.OrderDetails.Add(orderDetail);
            _context.SaveChanges();
        }

        public void Add(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public void Edit(Book book)
        {
            _context.Entry(book).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Edit(User User)
        {
            _context.Entry(User).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Remove(Book book)
        {
            _context.Entry(book).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
