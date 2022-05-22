using HyperShop.DataAccess;
using HyperShop.Models;
using HyperShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HyperShop.Web.Views.Components
{
    public class NavigationViewComponent
        : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public NavigationViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var model = new NavVM()
            {
                Brands = _context.Products
                .Include(p => p.Brand)
                .ToList()
                .GroupBy(p => p.Brand)
                .OrderByDescending(grp => grp.Count())
                .Select(grp => new Brand
                {
                    Id = grp.Key.Id,
                    Name = grp.Key.Name,
                })
                .Take(5),
                Categories = _context.Products
                .Include(p => p.Category)
                .ToList()
                .GroupBy(p => p.Category)
                .OrderByDescending(grp => grp.Count())
                .Select(grp => new Category
                {
                    Id = grp.Key.Id,
                    Name = grp.Key.Name,
                })
                .Take(5),

            };

            return View(model);
        }
    }
}
