using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using TMDT.DataAccess.Repository.IRepository;
using TMDT.Models;
using TMDT.Utility;

namespace Project_ThuongMaiDT.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork; //thuô?c ti?nh cu?a class na?y 

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(string searchTerm)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if(claim != null)
            {
                HttpContext.Session.SetInt32(SD.SessionCart,
                    _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value).Count());
            }
            // L?y danh sách s?n ph?m và bao g?m thu?c tính "Category"
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category");

            // N?u có t? khóa tìm ki?m, l?c danh sách s?n ph?m theo tiêu ?? ho?c tên tác gi?
            if (!string.IsNullOrEmpty(searchTerm))
            {
                productList = productList.Where(p => p.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            // L?u tr? t? khóa tìm ki?m trong ViewData ?? hi?n th? l?i trong thanh tìm ki?m
            ViewData["CurrentFilter"] = searchTerm;

            return View(productList);
        }

        public IActionResult Details(int productId)
        {
            ShoppingCart cart = new()
            {
                Product = _unitOfWork.Product.Get(u => u.Id == productId, includeProperties: "Category"),
                Count = 1,
                ProductId = productId,
            };
            return View(cart);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;
            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.ApplicationUserId == userId && 
            u.ProductId == shoppingCart.ProductId);
            if (cartFromDb != null)
            {
                cartFromDb.Count += shoppingCart.Count;
                //_unitOfWork.ShoppingCart.Update(shoppingCart); ?? ?ây nê?u du?ng câ?y na?y se? thêm mô?t c?? s?? d?? liê?u m??i v??i ID m??i ta?i
                //vi? khi thêm 1 sa?n phâ?m shoppongcart se? t?? sinh kho?a va? ta?o mô?t csdl m??i nh?ng ta chi? muô?n câ?p nhâ?t mô?t csdl ta muô?n câ?p nhâ?t ch?? không câ?n thêm 
                //d?? liê?u m??i
                _unitOfWork.ShoppingCart.Update(cartFromDb);
                _unitOfWork.Save();
                //vi?c không c?n g?i ph??ng th?c Update là do Entity Framework t? ??ng theo dõi các thay ??i ??i v?i các ??i t??ng ?ã n?p t? c? s? d? li?u
                //và s? áp d?ng nh?ng thay ??i này khi b?n l?u vào c? s? d? li?u b?ng cách g?i _unitOfWork.Save().
            }
            else
            {
                _unitOfWork.ShoppingCart.Add(shoppingCart);
                _unitOfWork.Save();
                HttpContext.Session.SetInt32(SD.SessionCart,
                    _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId).Count());
            }
            TempData["success"] = "Cart updated successfully";
            return RedirectToAction(nameof(Index));
        }
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