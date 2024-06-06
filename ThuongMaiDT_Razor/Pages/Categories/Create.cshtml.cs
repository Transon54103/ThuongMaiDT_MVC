using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ThuongMaiDT_Razor.Data;
using ThuongMaiDT_Razor.Model;

namespace ThuongMaiDT_Razor.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public Category Category { get; set; }

        public CreateModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet()
        {
        }
        public IActionResult OnPost() 
        {

            _db.categories.Add(Category);
            _db.SaveChanges();
            return RedirectToPage("Index");
        } 
    }
}
