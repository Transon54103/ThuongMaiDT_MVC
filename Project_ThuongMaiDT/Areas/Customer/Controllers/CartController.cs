using Microsoft.AspNetCore.Mvc;

namespace Project_ThuongMaiDT.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
