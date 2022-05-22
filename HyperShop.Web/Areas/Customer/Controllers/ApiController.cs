using HyperShop.DataAccess;
using HyperShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public IActionResult Product(int page,int taking, string search, string orderBy,bool isDesc, List<int> categories, List<int> brands,
            List<int> colors, List<string> shoesHeights, List<string> closureTypes, List<string> genders)
        {
            if (page == 0) page = 1;
            if (taking == 0) taking = 12;
            if (orderBy == null) orderBy = "Price";

            IQueryable<Product> products = _context.Products;

            if (search != null && search !="") products = products.Where(p => p.Name.Contains(search));
            if (categories != null && categories.Count>0) products = products.Where(p => categories.Contains(p.CategoryId.Value));
            if (brands != null && brands.Count > 0) products = products.Where(p => brands.Contains(p.BrandId.Value));
            if (shoesHeights != null && shoesHeights.Count > 0) products = products.Where(p => shoesHeights.Contains(p.ShoesHeight));
            if (closureTypes != null && closureTypes.Count > 0) products = products.Where(p => closureTypes.Contains(p.ClosureType));
            if (genders != null && genders.Count > 0) products = products.Where(p => genders.Contains(p.Gender));
            if (colors != null && colors.Count > 0) products = products.Where(p => _context.Stock.Any(s => s.ProductId == p.Id && colors.Contains(s.ColorId)));

            int productsCount = products.Count();

            if(orderBy=="Name")
            {
                if (isDesc) products = products.OrderByDescending(p => p.Name);
                else products = products.OrderBy(p => p.Name);

            }
            else if( orderBy == "Price")
            {
                if (isDesc) products = products.OrderByDescending(p => p.Price);
                else products = products.OrderBy(p => p.Price);
            }
            else if (orderBy == "PublishedDate")
            {
                if (isDesc) products = products.OrderByDescending(p => p.PublishedDate);
                else products = products.OrderBy(p => p.PublishedDate);
            }


            //Linq cant translate this to sql. Cant load before order due to performance
            //if (isDesc) products = products.OrderByDescending(p => p.GetType().GetProperty(orderBy).GetValue(p));
            //else products = products.OrderBy(p => p.GetType().GetProperty(orderBy).GetValue(p));

            var pagedProducts = products.ToList()
                .Skip(taking * (page - 1))
                .Take(taking);

            var productList = pagedProducts
                .Select(p => new {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    PrimaryImage = p.PrimaryImage,
                    ColorCount = _context.Stock.ToList()
                        .GroupBy(s => new { s.ProductId, s.ColorId })
                        .Where(s => s.Key.ProductId == p.Id)
                        .Count()
                });
            return Json(new { productList, productsCount });
        }
        
        public IActionResult Stock(int productId, int colorId)
        {
            var images = _context.SecondaryImages
                .Where(i => i.ProductId == productId && i.ColorId == colorId)
                .Select(i=> i.Url)
                .ToList();
            images.Insert(0,
                _context.PrimaryImages
                .First(i => i.ProductId == productId && i.ColorId == colorId).Url);

            var sizes = _context.Stock
                .Include(s=>s.Size)
                .Where(s => s.ProductId == productId && s.ColorId == colorId && s.Quantity>0)
                .Select(s=>new {s.SizeId, s.Size.SizeValue})
                .ToList();
            return Json(new { images, sizes });
        }
    }
}
