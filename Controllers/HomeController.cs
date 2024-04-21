using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MyBook.Data;
using MyBook.Models;
using System.Diagnostics;

namespace MyBook.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _imgPath;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext applicationDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _context = applicationDbContext;
            _webHostEnvironment = webHostEnvironment;
            _imgPath = $"{_webHostEnvironment.WebRootPath}{"/Images"}";
        }
        #region Home
        public IActionResult Index()
        {
            List<Book> books = _context.Books.ToList();
            return View(books);
        }

        #endregion

        #region Create
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveAdd(Book newBook)
        {
            if (ModelState.IsValid)
            {
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + newBook.ImageFile.FileName;
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", uniqueFileName);
                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    newBook.ImageFile.CopyTo(fileStream);
                }
                newBook.Image = uniqueFileName;
                _context.Books.Add(newBook);
                _context.SaveChanges();
                return RedirectToAction("Index");

            }
            return View("Add",newBook);            
        }

        #endregion

        #region Read
        public IActionResult Details(int id)
        {
            Book book = _context.Books.Find(id);
            return View(book);
        }
        #endregion

        #region Update
        public IActionResult Edit(int id)
        {
            Book book = _context.Books.Find(id);
            return View(book);
        }
        public IActionResult SaveEdit(Book book)
        {
            if (ModelState.IsValid)
            {
                var uniqueFileName = Guid.NewGuid().ToString();
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", uniqueFileName);
                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    book.ImageFile.CopyTo(fileStream);
                }
                book.Image = uniqueFileName;

                Book oldBook = _context.Books.Find(book.Id);
                if(oldBook == null)
                    return NotFound();
                oldBook.Image = book.Image;
                oldBook.Description = book.Description;
                oldBook.Title = book.Title;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Edit",book);
        }
        #endregion

        #region Delete
        public IActionResult Delete(int id)
        {
            Book book = _context.Books.Find(id);
            _context.Books.Remove(book);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
