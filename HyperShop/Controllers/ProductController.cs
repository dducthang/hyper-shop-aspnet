using Microsoft.AspNetCore.Mvc;

namespace HyperShop.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
