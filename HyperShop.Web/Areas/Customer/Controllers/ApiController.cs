using HyperShop.DataAccess;
using HyperShop.Models;
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

        public IActionResult Products(int page,int taking, string orderBy, List<int> categories, List<int> brands, List<int> colors, List<string> shoesHeights, List<string> closureTypes, List<string> genders)
        {
            if (page == 0) page = 1;
            if (taking == 0) taking = 12;
            if (orderBy == null) orderBy = "PublishedDate";

            IEnumerable<Product> products = _context.Products;

            if (categories != null && categories.Count>0) products = products.Where(p => categories.Contains(p.CategoryId.Value));
            if (brands != null && brands.Count > 0) products = products.Where(p => brands.Contains(p.BrandId.Value));
            if (shoesHeights != null && shoesHeights.Count > 0) products = products.Where(p => shoesHeights.Contains(p.ShoesHeight));
            if (closureTypes != null && closureTypes.Count > 0) products = products.Where(p => closureTypes.Contains(p.ClosureType));
            if (genders != null && genders.Count > 0) products = products.Where(p => genders.Contains(p.Gender));
            if (colors != null && colors.Count > 0) products = products.Where(p => _context.Stock.Any(s => s.ProductId == p.Id && colors.Contains(s.ColorId)));

            int productsCount = products.Count();

            products = products.OrderBy(p => p.GetType().GetProperty(orderBy).GetValue(p))
                .Skip(taking * (page - 1))
                .Take(taking);

            var productList = products
                .Select(p=> new { 
                    Id=p.Id,
                    Name=p.Name,
                    Price = p.Price,
                    PrimaryImage = p.PrimaryImage,
                    ColorCount= _context.Stock.ToList()
                        .GroupBy(s => new { s.ProductId, s.ColorId })
                        .Where(s => s.Key.ProductId == p.Id)
                        .Count()
                })
                .ToList();


            return Json(new { productList, productsCount });
        }
    }
}
