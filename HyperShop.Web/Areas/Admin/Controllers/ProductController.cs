using HyperShop.DataAccess;
using HyperShop.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HyperShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private  IWebHostEnvironment _hostEnvironment;
        private ApplicationDbContext _context;
        public ProductController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }


        //GET Product/Create
        public IActionResult Create()
        {
            
            ViewBag.CategoryList = _context.Categories.Select(c =>
                new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });
            ViewBag.BrandList = _context.Brands.Select(c =>
                new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });

            return View();
        }

        //POST Product/Create
        [HttpPost]
        public IActionResult Create(Product product, IFormFile? file)
        {   

            if(ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"img\products");
                var extension = Path.GetExtension(file.FileName);

                using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    file.CopyTo(fileStreams);
                }
                product.PrimaryImage = @"\images\products\" + fileName + extension;

                _context.Products.Add(product);
                _context.SaveChanges();
                return RedirectToAction("Index");   
            }
            return View(product);

        }





        #region API CALLS

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var productList = await _context.Products.ToListAsync();
            return Json(new { data = productList });
        }

        #endregion

    }
}
