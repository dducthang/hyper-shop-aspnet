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
                product.PrimaryImage = @"\img\products\" + fileName + extension;

                _context.Products.Add(product);
                _context.SaveChanges();
                return RedirectToAction("Index");   
            }
            return View(product);

        }



        //GET Product/Create
        public IActionResult Edit(int id)
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

            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product is not null)
            {
                return View(product);

            }
            else return NotFound();
        }

        //POST Product/Create
        [HttpPost]
        public IActionResult Edit(Product product, IFormFile? file)
        {

            if (ModelState.IsValid)
            {
                if(file != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"img\products");
                    var extension = Path.GetExtension(file.FileName);

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }

                    if (product.PrimaryImage != null)
                    {
                        string oldImage = Path.Combine(wwwRootPath, product.PrimaryImage.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImage))
                        { 
                            System.IO.File.Delete(oldImage);

                        }

                    }

                    product.PrimaryImage = @"\img\products\" + fileName + extension;

                }

                _context.Products.Update(product);
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
