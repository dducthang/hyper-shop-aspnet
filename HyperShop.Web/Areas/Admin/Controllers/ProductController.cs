using HyperShop.DataAccess;
using HyperShop.Models;
using HyperShop.Utility;
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
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product, IFormFile? file)
        {   

            if(ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"img\products");
                var extension = Path.GetExtension(file.FileName);
                string filePath = Path.Combine(uploads, fileName + extension);
                using (var fileStreams = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStreams);
                }
                ImageTool.Image_resize(filePath, filePath);

                product.PrimaryImage = @"\img\products\" + fileName + extension;

                _context.Products.Add(product);
                _context.SaveChanges();
                TempData["success"] = "Product created succesfully";
                return RedirectToAction("Index", "Stock", new { productId = product.Id });
            }
            return View(product);

        }



        //GET Product/Edit
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

        //POST Product/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product, IFormFile? file)
        {

            if (ModelState.IsValid)
            {
                if(file != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString();
                    string uploads = Path.Combine(wwwRootPath, @"img\products");
                    string extension = Path.GetExtension(file.FileName);
                    string filePath = Path.Combine(uploads, fileName + extension);
                    using (var fileStreams = new FileStream(filePath, FileMode.Create))
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

                    ImageTool.Image_resize(filePath, filePath);

                    product.PrimaryImage = @"\img\products\" + fileName + extension;

                }

                _context.Products.Update(product);
                _context.SaveChanges();
                TempData["success"] = "Product edited succesfully";

                return RedirectToAction("Index");
            }
            return View(product);

        }




        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _context.Products.ToList();
            return Json(new { data = productList });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if(product!= null)
            {
                //TODO: remove all image!!

                if (product.PrimaryImage != null)
                {
                    string oldImage = Path.Combine(_hostEnvironment.WebRootPath, product.PrimaryImage.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImage))
                    {
                        System.IO.File.Delete(oldImage);

                    }
                }
                _context.Products.Remove(product);
                _context.SaveChanges();
                return Json(new { success = true, message = "Delete successfully!" });
            }
            return Json(new { success = false, message = "Error while deleting!" });


        }
        #endregion

    }
}
