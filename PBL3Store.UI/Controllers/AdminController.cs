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
    public class AdminController : Controller
    {
        private readonly IMainRepository _mainRepository;
        public AdminController(IMainRepository mainRepository)
        {
            _mainRepository = mainRepository;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ViewResult BookList()
        {
            AdminBookListModel model = new AdminBookListModel();
            model.Books = _mainRepository.Books.OrderByDescending(x=>x.BookId).ToList();
            return View(model);
        }
        public ViewResult BookView(int id)
        {
            Book book = _mainRepository.Books.FirstOrDefault(x => x.BookId == id);
            return View(book);
        }
    }
}