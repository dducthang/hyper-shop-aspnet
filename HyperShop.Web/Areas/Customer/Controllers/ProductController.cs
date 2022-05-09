using Microsoft.AspNetCore.Mvc;

namespace HyperShop.Web.Areas.Customer.Controllers
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
