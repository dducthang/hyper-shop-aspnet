using Microsoft.AspNetCore.Mvc;

namespace HyperShop.Web.Areas.Customer.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
