using Microsoft.AspNetCore.Mvc;

namespace HyperShop.Controllers
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
