using HyperShop.DataAccess;
using HyperShop.Models;
using HyperShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HyperShop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            ProductsPageVM productsPageVM = new ProductsPageVM()
            {
                Brands = _context.Products
                .Include(p => p.Brand)
                .ToList()
                .GroupBy(p => p.Brand)
                
                .OrderByDescending(grp => grp.Count())
                .Take(5)
                
                .Select(p => new Brand()
                {
                    Id = p.Key.Id,
                    Name = p.Key.Name
                }).ToList(),
                Categories = _context.Products
                .Include(p => p.Category)
                .ToList()
                .GroupBy(p => p.Category)
                .OrderByDescending(grp => grp.Count())
                .Take(5)
                .Select(p => new Category()
                {
                    Id = p.Key.Id,
                    Name = p.Key.Name
                }).ToList(),

                Colors = _context.Stock
                .Include(p => p.Product)
                .Include(p => p.Color)
                .ToList()
                .GroupBy(s => new { s.Product, s.Color })
                .GroupBy(s => s.Key.Color)
                .OrderByDescending(grp => grp.Count())
                .Take(5)
                .Select(p => new Color { Id = p.Key.Id, ColorValue = p.Key.ColorValue})
                .ToList(),

                ShoesHeightTypes = _context.Products
                .ToList()
                .GroupBy(p => p.ShoesHeight)
                .OrderByDescending(grp => grp.Count())
                .Take(5)
                .Select(p => p.Key).ToList(),

                ClosureTypes = _context.Products.ToList()
                .GroupBy(p => p.ClosureType)
                .OrderByDescending(grp => grp.Count())
                .Take(5)
                .Select(p => p.Key).ToList(),

                Genders = _context.Products.ToList()
                .GroupBy(p => p.Gender)
                .OrderByDescending(grp => grp.Count())
                .Take(5)
                .Select(p => p.Key).ToList(),
            };
            return View(productsPageVM);
        }
        public IActionResult Detail(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product != null) return View(product);
            return NotFound();
        }
    }
}
