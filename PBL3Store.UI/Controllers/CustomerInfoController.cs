using PBL3Store.Domain;
using PBL3Store.Domain.Repositories;
using PBL3Store.UI.Infratructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PBL3Store.UI.Controllers
{
    public class CustomerInfoController : Controller
    {
        private readonly IMainRepository _mainRepository;
        private readonly IDbQueries _query;
        public CustomerInfoController(IMainRepository mainRepository, IDbQueries dbQueries)
        {
            _mainRepository = mainRepository;
            _query = dbQueries;
        }
        // GET: CustomerInfo
        public ActionResult UserRecord()
        {
            string UserName = User.Identity.Name;
            ViewBag.Shipper = _mainRepository.Shippers.ToList();
            User user = _mainRepository.Users.FirstOrDefault(x => x.UserName == UserName);
            if (user != null)
            {
                return View(user);
            }
            return RedirectToAction(nameof(AccountController.Login));
        }
    }
}