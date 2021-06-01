using PBL3Store.Domain;
using PBL3Store.Domain.Cart;
using PBL3Store.Domain.Repositories;
using PBL3Store.UI.Attributes;
using PBL3Store.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PBL3Store.UI.Controllers
{
    public class CartController : Controller
    {
        private readonly IMainRepository _mainRepository;
        public CartController(IMainRepository mainRepository)
        {
            _mainRepository = mainRepository;
        }
        // GET: Cart
        public ActionResult Index()
        {
            if (User.Identity.Name == "")
            {
                return Redirect("/Account/Login/");
            }
            Cart cart = GetCart();
            return View(cart);
        }
        [HttpPost]
        public ActionResult AddToCart(LineCartModel model)
        {   
            if(ModelState.IsValid)
            {
                Book book = _mainRepository.Books.FirstOrDefault(x => x.BookId == model.BookId);
                if(book!= null)
                {
                    Cart cart = GetCart();
                    cart.Add(book, model.Quantity);
                    return Json(new { state = true, message = "Thêm sách thành công" });
                }    
            }
            return Json(new { state = false, message = "Thêm sách thất bại" });
        }
        [HttpPost]
        public ActionResult UpdateToCart(LineCartModel model)
        {
            if (ModelState.IsValid)
            {
                var product = _mainRepository.Books.FirstOrDefault(x => x.BookId == model.BookId);
                if (product != null)
                {
                    var cart = GetCart();
                    cart.Update(product, model.Quantity);
                    //return Json(new { state = true, message = " thành công" });
                }
            }
            //return Json(new { state = false, message = "fail" });
            return Redirect("Cart/Index");
        }
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            var product = _mainRepository.Books.FirstOrDefault(x => x.BookId == id);
            if (product != null)
            {
                var cart = GetCart();
                cart.Remove(product);
            }
            return RedirectToAction("Index");
        }
        public PartialViewResult CartSummary()
        {
            Cart cart = GetCart();
            return PartialView(cart);
        }
        public PartialViewResult CartUpdateSummary()
        {
            Cart cart = GetCart();
            return PartialView(cart);
        }
        public ActionResult Checkout()
        {
            ViewBag.PaymentMethod = _mainRepository.Payments.ToList();
            string UserName = User.Identity.Name;
            User CurrentUser = _mainRepository.Users.FirstOrDefault(x => x.UserName == UserName);
            if (CurrentUser == null)
            {
                return Redirect("/Account/Login");
            }
            if (CurrentUser.Address == null)
            {
                return Redirect("/Account/UpdateInfo");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Checkout(CartCheckoutModel model)
        {
            ViewBag.PaymentMethod = _mainRepository.Payments.ToList();
            Cart cart = GetCart();
            if(cart.lines==null)
            {
                TempData["msg"] = "giỏ hàng rỗng vui lòng thêm sách để đặt hàng";
                return Redirect("/Cart/Checkout");
            }    
            if(model.paymentId == null)
            {
                ModelState.AddModelError("", "vui lòng chọn phương thức thanh toán");
                return View();
            }
            string UserName = User.Identity.Name;
            User CurrentUser = _mainRepository.Users.FirstOrDefault(x => x.UserName == UserName);
            if(CurrentUser == null)
            {
                return Redirect("/Account/Login");
            }
            Order order = new Order()
            {
                CreateDate = DateTime.Now,
                Note = model.Note,
                PaymentId = model.paymentId,
                UserId = CurrentUser.UserId,
                StateId = 1
            };
            _mainRepository.Add(order);
            foreach(CartLine line in cart.lines)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    BookId = line.Book.BookId,
                    OrderId = order.OrderId,
                    Price = line.Book.Price,
                    Quantity = line.Quantity
                };
                Book book = _mainRepository.Books.FirstOrDefault(x => x.BookId == line.Book.BookId);
                if(book.Quantity - line.Quantity < 0)
                {
                    ModelState.AddModelError("", $"Sách {book.BookName} chỉ còn {book.Quantity} cuốn vui lòng điều chỉnh số lượng hoặc chọn sách khác!");
                    return View(model);
                }       
                book.Quantity -= line.Quantity;
                if(book.Quantity == 0)
                {
                    book.State = false;
                }    
                _mainRepository.Edit(book);
                _mainRepository.Add(orderDetail);
            }
            cart.Clear();
            TempData["msg"] = "Đặt hàng thành công xin vui lòng chờ hàng được chuyển đến";
            return Redirect("/Home/HomePage");
        }
        #region CartHelper
        private Cart GetCart()
        {
            var cart = Session["cart"] as Cart;
            if (cart == null)
            {
                cart = new Cart();
                Session["cart"] = cart;
            }
            return cart;
        }
        #endregion
    }

}