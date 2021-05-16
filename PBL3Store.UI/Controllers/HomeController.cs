using PBL3Store.Domain;
using PBL3Store.Domain.Repositories;
using PBL3Store.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PBL3Store.UI.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        private IMainRepository _mainRepository;
        public HomeController(IMainRepository mainRepository)
        {
            _mainRepository = mainRepository;
        }
        public ViewResult HomePage(int page=1, int pageSize=10, int categoriId=-1)
        {
            HomeListBookModel model = new HomeListBookModel();
            model.Books = _mainRepository.Books
                .OrderByDescending(x => x.BookId)
                .Skip((page - 1) * pageSize).Where(x => x.CategoryId == categoriId || categoriId == -1)
                .ToList();
            return View(model);
        }
        public ViewResult BookDetail(int BookId)
        {
            Book book = _mainRepository.Books.FirstOrDefault(x => x.BookId == BookId);
            if(book!= null)
            {
                return View(book);
            }
            return View("NotFound");
        }

        public ActionResult OrderManage()
        {
            string UserName = User.Identity.Name;
            User customer = _mainRepository.Users.FirstOrDefault(x => x.UserName == UserName);
            if (customer != null)
            {
                List<Order> orders = _mainRepository.order.Where(x => x.UserId == customer.UserId).ToList();
                if (orders != null)
                {
                    HomeOrderManageModel model = new HomeOrderManageModel()
                    {
                        Orders = orders,
                        customer = customer
                    };
                    ViewBag.PaymentMethod = _mainRepository.Payments.ToList();
                    return View(model);
                }
            }
            return View("NotFound");
        }
        public ActionResult ViewOrder(int OrderId)
        {
            List<OrderDetail> ordetail = _mainRepository.OrderDetails.Where(x => x.OrderId == OrderId).ToList();
            if (ordetail != null)
            {
                HomeViewOrderModel model = new HomeViewOrderModel();
                model.orderDetails = ordetail;
                ViewBag.Book = _mainRepository.Books.ToList();
                return View(model);
            }
            return View();
        }
    }
}