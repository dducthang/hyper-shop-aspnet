using HyperShop.DataAccess;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace HyperShop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]

    public class ApiController : Controller
    {
        private ApplicationDbContext _context;

        public ApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Products(int page, List<int> categories, List<int> bands, List<int> colors, List<string> shoesHeights, List<string> closureTypes, List<string> genders)
        {
            var products = _context.Products.Skip(3 * page).Take(3).ToList();
            return Json(products);
        }
    }
}
