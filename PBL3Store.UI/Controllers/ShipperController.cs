using PBL3Store.Domain;
using PBL3Store.Domain.Repositories;
using PBL3Store.UI.Attributes;
using PBL3Store.UI.Infratructure;
using PBL3Store.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PBL3Store.UI.Controllers
{
    [CustomFilterRole(RolesAllow ="Shipper,Admin")]
    public class ShipperController : Controller
    {
        private readonly IMainRepository _mainRepository;
        private readonly IDbQueries _query;
        public ShipperController(IMainRepository mainRepository, IDbQueries dbQueries)
        {
            _mainRepository = mainRepository;
            _query = dbQueries;
        }
        public ViewResult ShipperViewOrder(HomeOrderManageModel model)
        {
            string ShipperName = User.Identity.Name;
            User Shipper = _mainRepository.Users.FirstOrDefault(x => x.UserName == ShipperName);
            ViewBag.PaymentMethod = _mainRepository.Payments.ToList();
            model.Orders = _mainRepository.order.Where(x => x.StateId == 1).ToList();
            model.customer = Shipper;
            return View(model);
        }
        public ViewResult ShipperOrderDetail(int OrderId)
        {
            ViewBag.Book = _mainRepository.Books.ToList();
            //List<OrderDetail> orderdetail = _mainRepository.OrderDetails.Where(x => x.OrderId == OrderId).ToList();
            List<OrderDetail> orderdetail = _query.GetViewOrder(OrderId);
            if (orderdetail != null)
            {
                HomeViewOrderModel model = new HomeViewOrderModel();
                model.orderDetails = orderdetail;
                return View(model);
            }
            return View();
        }
        [HttpPost]
        public ActionResult GetOrder(HomeOrderModel model)
        {
            string ShipperName = User.Identity.Name;
            User Shipper = _mainRepository.Users.FirstOrDefault(x => x.UserName == ShipperName);
            Shipper sp = _mainRepository.Shippers.FirstOrDefault(x => x.UserId == Shipper.UserId);
            Order order = _mainRepository.order.FirstOrDefault(x => x.OrderId == model.OrderId);
            if(order == null || Shipper == null)
            {
                ModelState.AddModelError("", "dữ liệu không hợp lệ");
                return RedirectToAction(nameof(ShipperController.ShipperViewOrder));
            }    
            if(order.StateId != 1)
            {
                ModelState.AddModelError("", "Đơn hàng đã được chuyển");
                return RedirectToAction(nameof(ShipperController.ShipperViewOrder));
            }
            order.StateId = 2;
            order.ShipperId = sp.ShipperId;
            _mainRepository.Edit(order);
            TempData["msgAdmin"] = "Nhận đơn hàng thành công";
            return RedirectToAction(nameof(ShipperController.ShipperViewOrder));
        }
        public ActionResult OrderTake(HomeOrderManageModel model)
        {
            string ShipperName = User.Identity.Name;
            User Shipper = _mainRepository.Users.FirstOrDefault(x => x.UserName == ShipperName);
            Shipper sp = _mainRepository.Shippers.FirstOrDefault(x => x.UserId == Shipper.UserId);
            ViewBag.PaymentMethod = _mainRepository.Payments.ToList();
            if (sp!= null)
            {
                model.Orders = _mainRepository.order.Where(x => x.ShipperId == sp.ShipperId).ToList();
                model.customer = Shipper;
                return View(model);
            }
            return RedirectToAction(nameof(AccountController.Login));
        }
        [HttpPost]
        public ActionResult CancelGet(HomeOrderModel model)
        {
            string ShipperName = User.Identity.Name;
            User Shipper = _mainRepository.Users.FirstOrDefault(x => x.UserName == ShipperName);
            Order order = _mainRepository.order.FirstOrDefault(x => x.OrderId == model.OrderId);
            if (order == null || Shipper == null)
            {
                ModelState.AddModelError("", "dữ liệu không hợp lệ");
                return RedirectToAction(nameof(ShipperController.OrderTake));
            }
            if (order.StateId != 2 && order.StateId != 3)
            {
                ModelState.AddModelError("", "Không thể hủy do đã hoàn thành đơn hàng");
                return RedirectToAction(nameof(ShipperController.OrderTake));
            }
            order.StateId = 1;
            order.ShipperId = null;
            _mainRepository.Edit(order);
            TempData["msgAdmin"] = "Hủy nhận đơn hàng thành công";
            return RedirectToAction(nameof(ShipperController.OrderTake));
        }
        [HttpPost]
        public ActionResult CompleteOrder(HomeOrderModel model)
        {
            Order order = _mainRepository.order.FirstOrDefault(x => x.OrderId == model.OrderId);
            if (order == null)
            {
                ModelState.AddModelError("", "dữ liệu không hợp lệ");
                return RedirectToAction(nameof(ShipperController.OrderTake));
            }
            if (order.StateId != 2 && order.StateId != 3)
            {
                ModelState.AddModelError("", "Đơn hàng đã hoàn thành");
                return RedirectToAction(nameof(ShipperController.OrderTake));
            }
            order.StateId = 4;
            _mainRepository.Edit(order);
            TempData["msgAdmin"] = "Hoàn thành giao hàng";
            return RedirectToAction(nameof(ShipperController.OrderTake));
        }
        public ActionResult Index()
        {
            string userName = User.Identity.Name;
            User currentUser = _mainRepository.Users.FirstOrDefault(x => x.UserName == userName);
            if(currentUser!=null)
            {
                
            }
            return View();
        }
    }
}