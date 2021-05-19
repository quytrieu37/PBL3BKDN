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
        public ViewResult HomePage(int page=1, int pageSize=12, int categoriId=-1)
        {
            HomeListBookModel model = new HomeListBookModel();
            model.Books = _mainRepository.Books
                .OrderByDescending(x => x.BookId).Where(x => x.State == true)
                .Skip((page - 1) * pageSize).Take(pageSize).Where(x => x.CategoryId == categoriId || categoriId == -1)
                .ToList();
            model.pagingInfo = new PagingInfo()
            {
                PageSize = pageSize,
                CurrentPage = page,
                TotalItem = _mainRepository.Books.Where(x => x.State == true).Count()
            };
                
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

        public ActionResult OrderManage(int page = 1, int pageSize = 10)
        {
            string UserName = User.Identity.Name;
            User customer = _mainRepository.Users.FirstOrDefault(x => x.UserName == UserName);
            if (customer != null)
            {
                List<Order> orders = _mainRepository.order.OrderBy(x=>x.OrderId).Where(x => x.UserId == customer.UserId).Skip((page-1)*pageSize).Take(pageSize).ToList();
                if (orders != null)
                {
                    HomeOrderManageModel model = new HomeOrderManageModel()
                    {
                        Orders = orders,
                        customer = customer,
                        pagingInfo = new PagingInfo()
                        {
                            PageSize = pageSize,
                            CurrentPage = page,
                            TotalItem = _mainRepository.order.Where(x => x.UserId == customer.UserId).Count()
                        }
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
        [HttpPost]
        public ActionResult CompleteOrder(HomeOrderModel model)
        {
            Order order = _mainRepository.order.FirstOrDefault(x => x.OrderId == model.OrderId);
            if( order== null)
            {
                ModelState.AddModelError("", "Đơn hàng không tồn tại");
                return RedirectToAction(nameof(HomeController.OrderManage));
            }
            order.StateId = 7;
            _mainRepository.Edit(order);
            TempData["msg"] = "Đã hoàn thành đơn hàng!";
            return RedirectToAction(nameof(HomeController.OrderManage));
        }
        public ActionResult RegisterShipper()
        {
            string UserName = User.Identity.Name;
            User user = _mainRepository.Users.FirstOrDefault(x => x.UserName == UserName);
            if(user!= null)
            {
                Shipper sp = _mainRepository.Shippers.FirstOrDefault(x => x.UserId == user.UserId);
                if (sp != null)
                {
                    TempData["msg"] = "Bạn đã đăng kí shipper";
                    return Redirect("/Home/HomePage/");
                }
                HomeRegisterUserModel model = new HomeRegisterUserModel();
                model.UserId = user.UserId;
                if(model.Address != null && model.Phone != null)
                {
                    model.Address = user.Address;
                    model.Phone = user.Phone;
                }
                return View(model);
            }    
            return View("NotFound");
        }
        [HttpPost]
        public ActionResult RegisterShipper(HomeRegisterUserModel model)
        {
            User user = _mainRepository.Users.FirstOrDefault(x => x.UserId == model.UserId);
            if(user != null)
            {
                if(ModelState.IsValid)
                {
                    user.Address = model.Address;
                    user.Phone = model.Phone;
                    _mainRepository.Edit(user);
                    Shipper shipper = new Shipper()
                    {
                        UserId = user.UserId,
                        CMND = model.CMND,
                        HomeTown = model.HomeTown,
                        ShipperName = model.ShipperName,
                        Phone = model.Phone
                    };
                    _mainRepository.Add(shipper);
                    TempData["msg"] = "Đăng kí shipper thành công, xin chờ phản hồi";
                    return Redirect("/Home/HomePage");
                }
                return View(model);
            }    
            return View(model);
        }
        public ActionResult RedirectUI()
        {
            string userName = User.Identity.Name;
            User currentUser = _mainRepository.Users.FirstOrDefault(x => x.UserName == userName);
            if(currentUser!= null)
            {
                if(currentUser.RoleId==1)
                {
                    return Redirect("/Admin/Index");
                }    
                if(currentUser.RoleId ==2 )
                {
                    return Redirect("/Shipper/Index");
                }
                else
                {
                    return Redirect("/Home/UserUI");
                }
            }
            return Redirect("/Account/Login");
        }
    }
}