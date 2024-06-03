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
        [HttpPost]
        public IActionResult Create(Category obj) 
        {
            if(obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name","The DisplayOrder cannot exactly match the Name.");
            }
            if (ModelState.IsValid)
            {
                _dB.categories.Add(obj);
                _dB.SaveChanges();
                TempData["success"] = "Category create successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category categoryFromDb = _dB.categories.Find(id);
            //Category categoryFromDb1 = _dB.categories.FirstOrDefault(u => u.Id==id);
            //Category categoryFromDb2 = _dB.categories.Where(u => u.Id==id).FirstOrDefault();
            if (categoryFromDb == null) { return NotFound(); }

            return View(categoryFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {   
            if (ModelState.IsValid)
            {
                _dB.categories.Update(obj);
                _dB.SaveChanges();
                TempData["success"] = "Category update successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category categoryFromDb = _dB.categories.Find(id);
            if (categoryFromDb == null) { return NotFound(); }

            return View(categoryFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category obj = _dB.categories.Find(id);
            if (obj == null) { return NotFound(); }
            _dB.categories.Remove(obj);
            _dB.SaveChanges();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
