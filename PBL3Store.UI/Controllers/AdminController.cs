using PBL3Store.Domain;
using PBL3Store.Domain.Repositories;
using PBL3Store.UI.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
                ViewBag.Categories = _mainRepository.Categories.ToList();
                _mainRepository.Add(newBook);
                TempData["msgAdmin"] = "Thêm sách thành công!";
                return Redirect("/Admin/BookList");
            }
            return View(model);
        }
        public ViewResult EditBook(int bookId)
        {
            Book book = _mainRepository.Books.FirstOrDefault(x => x.BookId == bookId);
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
    }
}