using PBL3Store.Domain;
using PBL3Store.Domain.Repositories;
using PBL3Store.UI.Attributes;
using PBL3Store.UI.Infratructure;
using PBL3Store.UI.Models;
using PBL3Store.UI.Models.Dynamic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PBL3Store.UI.Controllers
{
    [CustomFilterRole(RolesAllow = "Admin,Shipper")]
    public class AdminController : Controller
    {
        private readonly IMainRepository _mainRepository;
        private readonly IDbQueries _query;
        public AdminController(IMainRepository mainRepository,
            IDbQueries query)
        {
            _mainRepository = mainRepository;
            _query = query;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ViewResult BookList()
        {
            AdminBookListModel model = new AdminBookListModel();
            model.Books = _query.GetAllBook();
            ViewBag.Categories = _mainRepository.Categories.ToList();
            return View(model);
        }
        public ViewResult BookView(int id)
        {
            //Book book = _mainRepository.Books.FirstOrDefault(x => x.BookId == id);
            ViewBag.Category = _mainRepository.Categories.ToList();
            Book book = _query.GetBookById(id);
            return View(book);
        }
        public ActionResult StopSell(int bookId)
        {
            Book book = _mainRepository.Books.FirstOrDefault(x => x.BookId == bookId);
            if(book!= null)
            {
                book.State = !book.State;
                _mainRepository.Edit(book);
                TempData["msgAdmin"] = "Đã thay đổi trạng thái sách";
                return Redirect("/Admin/BookList");
            }    
            return Redirect("/Admin/BookList");
        }
        public ViewResult AddNewBook()
        {
            ViewBag.Categories = _mainRepository.Categories.ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewBook(AdminAddNewBookModel model)
        {
            ViewBag.Categories = _mainRepository.Categories.ToList();
            var files = Request.Files;
            if(files.Count ==0)
            {
                ModelState.AddModelError("", "Vui lòng thêm hình ảnh để minh họa sách");
                return View(model);
            }    
            if(ModelState.IsValid)
            {
                string[] ExtentionAllow = new string[] { ".jpg", ".png", ".jpeg" };
                var file = files[0];
                string ext = Path.GetExtension(file.FileName).ToLower();
                if(!ExtentionAllow.Any(x=>x == ext))
                {
                    ModelState.AddModelError("", "Tệp tin không hợp lệ");
                    return View(model);
                }
                string folder = "/Content/Upload/";
                string path = Server.MapPath("~" + folder);
                if(!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string fullPath = Path.Combine(path,file.FileName);
                file.SaveAs(fullPath);
                Book newBook = new Book()
                {
                    BookName = model.BookName,
                    Author = model.Author,
                    CategoryId = model.CategoryId,
                    Description = model.Description,
                    Price = model.Price,
                    Quantity = model.Quantity,
                    State = model.State,
                    BookImage = "/Content/Upload/" + file.FileName
                };
                
                _mainRepository.Add(newBook);
                TempData["msgAdmin"] = "Thêm sách thành công!";
                return Redirect("/Admin/BookList");
            }
            return View(model);
        }
        public ViewResult EditBook(int bookId)
        {
            //Book book = _mainRepository.Books.FirstOrDefault(x => x.BookId == bookId);
            Book book = _query.GetBookById(bookId);
            ViewBag.Categories = _mainRepository.Categories.ToList();
            if (book != null)
            {
                AdminEditBookModel model = new AdminEditBookModel()
                {
                    BookId = book.BookId,
                    BookName = book.BookName,
                    Author = book.Author,
                    Avatar = book.BookImage,
                    CategoryId = book.CategoryId,
                    Description = book.Description,
                    Price = book.Price,
                    Quantity = book.Quantity,
                    State = (bool)book.State
                };
                return View(model);
            }    
            return View("NotFound");
        }
        [HttpPost]
        public ActionResult EditBook(AdminEditBookModel model)
        {
            ViewBag.Categories = _mainRepository.Categories.ToList();
            if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
            {
                string[] extention = new string[] { ".jpg", ".png", ".jpeg" };
                var file = Request.Files[0];
                string ext = Path.GetExtension(file.FileName);
                if (!extention.Any(x => x == ext))
                {
                    ModelState.AddModelError("", "file tải lên không hợp lệ");
                    return View(model);
                }
                else
                {
                    string folder = "/Content/Upload/";
                    string path = Server.MapPath("~" + folder);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string result = Path.Combine(path, file.FileName);
                    file.SaveAs(result);
                    model.Avatar = "/Content/Upload/" + file.FileName;

                    if (ModelState.IsValid)
                    {
                        Book book = new Book()
                        {
                            Author = model.Author,
                            Description = model.Description,
                            Price = model.Price,
                            BookName = model.BookName,
                            BookImage = model.Avatar,
                            CategoryId = model.CategoryId,
                            Quantity = model.Quantity,
                            State = model.State,
                            BookId = ((int)model.BookId)
                        };
                        _mainRepository.Edit(book);
                        TempData["msgAdmin"] = "Chỉnh sửa sách thành công";
                        return RedirectToAction(nameof(AdminController.BookList));
                    }
                }
            }
            return View(model);
        }
        public ViewResult ListUser(AdminListUserModel model)
        {
            ViewBag.Shippers = _mainRepository.Shippers.ToList();
            model.Users = _mainRepository.Users.ToList();
            return View(model);
        }
        public ViewResult UserDetail(int UserId)
        {
            ViewBag.PaymentMethod = _mainRepository.Payments.ToList();
            ViewBag.Shippers = _mainRepository.Shippers.ToList();
            User user = _mainRepository.Users.FirstOrDefault(x => x.UserId == UserId);
            //User user = _query.GetUserById(UserId);
            AdminUserDetailModel model = new AdminUserDetailModel();
            if(user != null)
            {
                model.user = user;
                if(user.RoleId == 2) // shipper
                {
                    model.Orders = _mainRepository.order.Where(x => x.ShipperId == user.UserId).ToList();
                    return View(model);
                }   
                else // Admin và khách hàng
                {
                    model.Orders = _mainRepository.order.Where(x => x.UserId == user.UserId).ToList();
                    return View(model);
                }    
            }    
            return View("NotFound");
        }
        [HttpPost]
        public ActionResult BlockUser(int UserId)
        {
            User user = _mainRepository.Users.FirstOrDefault(x => x.UserId == UserId);
            if(user!=null)
            {
                if(user.StateId == 8)
                {
                    user.StateId = 9;
                }    
                else
                {
                    user.StateId = 8;
                }    
                _mainRepository.Edit(user);
                return RedirectToAction(nameof(AdminController.ListUser));
            }
            return View("NotFound");
        }
        public ActionResult BookRevenue(int status)
        {
            AdminBookRevenueModel model = new AdminBookRevenueModel();
            model.BookRevenues = _mainRepository.OrderDetails.GroupBy(x => x.BookId).Select(c1 => new BookRevenue
            {
                BookId = c1.Select(x => x.BookId).FirstOrDefault(),
                BookName = c1.Select(x => x.Book.BookName).FirstOrDefault(),
                Quantity = c1.Sum(x => x.Quantity),
                Price = c1.Select(x => x.Price).FirstOrDefault()
            }).ToList();
            if(status == 0 )
            {
                return View(model);
            }    
            else if(status==1)
            {
                model.BookRevenues = model.BookRevenues.OrderBy(x => x.Quantity).ToList();
                return View(model);
            }
            else
            {
                model.BookRevenues = model.BookRevenues.OrderByDescending(x => x.Quantity).ToList();
                return View(model);
            }
        }
        public ActionResult Revenues(DateTime start, DateTime end)
        {
            return View();
        }
        public ActionResult ListOrder()
        {
            HomeOrderManageModel model = new HomeOrderManageModel();
            ViewBag.PaymentMethod = _mainRepository.Payments.ToList();
            model.Orders = _mainRepository.order.Where(x => x.StateId == 1).ToList();
            return View(model);
        }
        public ActionResult AuthoShipper()
        {
            List<Shipper> list = _mainRepository.Shippers.ToList();
            AdminListUserModel model = new AdminListUserModel();
            model.Users = new List<User>();
            ViewBag.Shippers = _mainRepository.Shippers.ToList();
            foreach (var sp in list)
            {
                User us = _mainRepository.Users.FirstOrDefault(x => x.UserId == sp.UserId);
                if(us!= null)
                {
                    if(us.RoleId==3)
                    {
                        model.Users.Add(us);
                    }    
                }    
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult AuthoShipper(int UserId)
        {
            ViewBag.Shippers = _mainRepository.Shippers.ToList();
            User user = _mainRepository.Users.FirstOrDefault(x => x.UserId == UserId);
            if(user != null)
            {
                user.RoleId = 2;
                _mainRepository.Edit(user);
                TempData["msgAdmin"] = "Đã thêm shipper";
                return Redirect("/Admin/ListUser");
            }
            return View();
        }
        [HttpGet]
        public ActionResult OrderAmount(int? state=null, DateTime? start =null, DateTime? end = null)
        {
            if (state == 1) // current month
            {
                start = DateTime.Now - new TimeSpan(30, 0, 0, 0);
                end = DateTime.Now;
            }
            if(state == 2) //current year
            {
                start = DateTime.Now - new TimeSpan(DateTime.Now.DayOfYear, 0, 0, 0);
                end = DateTime.Now;
            }
            List<Order> orders = _query.GetOrderBaseMileStones(start, end);
            ViewBag.PaymentMethod = _mainRepository.Payments.ToList();
            ViewBag.State = _mainRepository.States.ToList();
            ViewBag.Shipper = _mainRepository.Shippers.ToList();
            return View(orders);
        }
    }
}