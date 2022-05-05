using Microsoft.AspNetCore.Mvc;

namespace HyperShop.Controllers
{
    public class ProductController : Controller
    {
        [Area("Customer")]

        public IActionResult Index()
        {
            return View();
        }
    }
}
