using Microsoft.AspNetCore.Mvc;
using Project_ThuongMaiDT.Data;
using Project_ThuongMaiDT.Models;

namespace Project_ThuongMaiDT.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _dB;
        public CategoryController(ApplicationDbContext db)
        {
            _dB = db;
        }
        public IActionResult Index()
        {
            List<Category> objcategorylist = _dB.categories.ToList();
            return View(objcategorylist);
        }
        public IActionResult Create() 
        {
            return View();
        }
    }
}
