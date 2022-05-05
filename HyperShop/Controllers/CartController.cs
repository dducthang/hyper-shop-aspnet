using Microsoft.AspNetCore.Mvc;

namespace HyperShop.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
