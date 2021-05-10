using PBL3Store.Domain;
using PBL3Store.Domain.Cart;
using PBL3Store.Domain.Repositories;
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
            Cart cart = GetCart();
            return View(cart);
        }
        [HttpPost]
        public JsonResult AddToCart(LineCartModel model)
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
                }
            }
            return RedirectToAction("Index");
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