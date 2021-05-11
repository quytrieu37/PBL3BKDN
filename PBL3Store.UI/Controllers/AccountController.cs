using PBL3Store.Domain;
using PBL3Store.Domain.Repositories;
using PBL3Store.UI.HashPassword;
using PBL3Store.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PBL3Store.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMainRepository _mainRepository;
        public AccountController(IMainRepository mainRepository)
        {
            _mainRepository = mainRepository;
        }
        // GET: Account
        public ViewResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(AccountRegisterModel model)
        {
            User User = _mainRepository.Users.FirstOrDefault(x => x.UserName == model.UserName);
            if (User != null)
            {
                ModelState.AddModelError("", "tên đăng nhập đã tồn tại");
                return View(model);
            }
            if (ModelState.IsValid)
            {
                string hashPassword = MD5Helper.HashMD5(model.Password);
                User user = new User()
                {
                    UserName = model.UserName,
                    Password = hashPassword,
                    Email = model.Email,
                    RoleId= 3
                };
                _mainRepository.Add(user);
                TempData["msg"] = "Tạo tài khoản thành công";
                return View("Login");
            }
            return View(model);
        }
        public ViewResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(AccountLoginModel model)
        {
            if(ModelState.IsValid)
            {
                User user = _mainRepository.Users.FirstOrDefault(x => x.UserName == model.UserName);
                if(user==null)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại");
                    return View(model);
                }
                if(MD5Helper.VerifyPass(user.Password, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, true);
                    return Redirect("/Home/HomePage");
                }
                ModelState.AddModelError("","Sai Mật khẩu");
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction(nameof(AccountController.Login));
        }
    }
}