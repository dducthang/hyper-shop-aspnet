using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HyperShop.DataAccess;
using HyperShop.Models;
using HyperShop.Models.ViewModels;
using HyperShop.Models.StockStuff;
using Microsoft.AspNetCore.Http;
using HyperShop.Utility;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace HyperShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StockController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment _hostEnvironment;


        public StockController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Admin/Stock?productId=3
        public IActionResult Index(int productId)
        {
            if (productId == 0) return NotFound();
            var stock = _context.Stock
                .Where(s => s.ProductId == productId)
                .Include(s => s.Color)
                .ToList()
                .GroupBy(s => new { s.ColorId, s.Color.ColorValue })
                .Select(s => new ColorStock
                {
                    Id = s.Key.ColorId,
                    ColorValue = s.Key.ColorValue,
                    
                });
            var stockList = stock.ToList();
            for (int i = 0; i < stockList.Count; i++)
            {
                var img = _context.PrimaryImages.FirstOrDefault(pi => pi.ProductId == productId && pi.ColorId == stockList[i].Id);
                if (img != null)
                {
                    stockList[i].Image = img.Url;
                }

            }
            var stockVM = new StockVM
            {
                ProductId = productId,
                Stock = stockList.ToList()
            };
            return View(stockVM);
        }

        

        // GET: Admin/Stock/Create
        public IActionResult Create(int productId)
        {
            List<SizeQuantity> sizeQty = _context.Sizes.Select(s => new SizeQuantity
            {
                Size = s.SizeValue,
                SizeId = s.Id,
                Qty = 0
            }).ToList();
            



            StockUpsertVM stockUpsertVM = new StockUpsertVM
            {
                ProductId = productId,
                SizeQty = sizeQty

            };


            ViewBag.Colors = _context.Colors.Where(s =>
            
                !_context.Stock.Any(c => c.ColorId == s.Id && c.ProductId == productId)
            ).Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.ColorValue
            });


            return View(stockUpsertVM);
        }

        // POST: Admin/Stock/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(StockUpsertVM stockUpsertVM, IFormFile? primaryImg, List<IFormFile> secondaryImg)
        {
            if (ModelState.IsValid)
            {
                //Add many stock rows
                foreach(var item in stockUpsertVM.SizeQty)
                {
                    if (item.Qty > 0)
                    {
                        _context.Stock.Add(new Stock
                        {
                            ProductId = stockUpsertVM.ProductId,
                            ColorId = stockUpsertVM.ColorId,
                            SizeId = item.SizeId,
                            Quantity = item.Qty
                        });
                    }
                }

                if(primaryImg!= null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    var uploads = Path.Combine(wwwRootPath, @"img\products");

                    string fileName = Guid.NewGuid().ToString();
                    var extension = Path.GetExtension(primaryImg.FileName);
                    string filePath = Path.Combine(uploads, fileName + extension);
                    using (var fileStreams = new FileStream(filePath, FileMode.Create))
                    {
                        primaryImg.CopyTo(fileStreams);
                    }
                    ImageTool.Image_resize(filePath, filePath);

                    //add a primary img
                    _context.PrimaryImages.Add(new PrimaryImage
                    {
                        ColorId = stockUpsertVM.ColorId,
                        ProductId = stockUpsertVM.ProductId,
                        Url = @"\img\products\" + fileName + extension
                    });

                    //add many secondary img rows
                    if(secondaryImg!= null)
                    {
                        foreach (var item in secondaryImg)
                        {
                            fileName = Guid.NewGuid().ToString();
                            extension = Path.GetExtension(item.FileName);
                            filePath = Path.Combine(uploads, fileName + extension);
                            using (var fileStreams = new FileStream(filePath, FileMode.Create))
                            {
                                item.CopyTo(fileStreams);
                            }
                            ImageTool.Image_resize(filePath, filePath);

                            _context.SecondaryImages.Add(new SecondaryImage
                            {
                                ColorId = stockUpsertVM.ColorId,
                                ProductId = stockUpsertVM.ProductId,
                                Url = @"\img\products\" + fileName + extension
                            });
                        }
                    }

                }
                _context.SaveChanges();
                return RedirectToAction("Index", new { productId = stockUpsertVM.ProductId });
            }
            return View(stockUpsertVM);
        }

        // GET: Admin/Stock/Edit?produtcId=8&&colorId=2
        public IActionResult Edit(int productId, int colorId)
        {
            List<SizeQuantity> sizeQty = _context.Sizes.Select(s => new SizeQuantity
            {
                Size = s.SizeValue,
                SizeId = s.Id,
                     
            }).ToList();

            for(int i =0; i < sizeQty.Count; i++)
            {
                var qty = _context.Stock.FirstOrDefault(st => st.ProductId == productId && st.ColorId == colorId && st.SizeId == sizeQty[i].SizeId);
                if (qty != null)
                {
                    sizeQty[i].Qty = qty.Quantity;
                    sizeQty[i].StockId = qty.Id;
                }

            }

            StockUpsertVM stockUpsertVM = new StockUpsertVM
            {
                ProductId = productId,
                SizeQty = sizeQty,
                ColorId = colorId

            };
            return View(stockUpsertVM);
        }

        // POST: Admin/Stock/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(StockUpsertVM stockUpsertVM, IFormFile? primaryImg, List<IFormFile> secondaryImg)
        {
            List<Stock> stockQty = new();
            foreach (var item in stockUpsertVM.SizeQty)
            {
                stockQty.Add(new Stock
                {
                    Id = item.StockId,
                    SizeId = item.SizeId,
                    Quantity = item.Qty,
                    ColorId = stockUpsertVM.ColorId,
                    ProductId = stockUpsertVM.ProductId,
                });
            }
            if (primaryImg != null)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString();
                string uploads = Path.Combine(wwwRootPath, @"img\products");
                string extension = Path.GetExtension(primaryImg.FileName);
                string filePath = Path.Combine(uploads, fileName + extension);
                using (var fileStreams = new FileStream(filePath, FileMode.Create))
                {
                    primaryImg.CopyTo(fileStreams);
                }
                var priImg = _context.PrimaryImages.FirstOrDefault(i => i.ProductId == stockUpsertVM.ProductId && i.ColorId == stockUpsertVM.ColorId);
                if (priImg != null)
                {
                    string oldImage = Path.Combine(wwwRootPath, priImg.Url.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImage))
                    {
                        System.IO.File.Delete(oldImage);

                    }

                }

                ImageTool.Image_resize(filePath, filePath);

                priImg.Url = @"\img\products\" + fileName + extension;

            }
            if (secondaryImg != null)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string uploads = Path.Combine(wwwRootPath, @"img\products");

                var oldImgList = _context.SecondaryImages.Where(i => i.ProductId == stockUpsertVM.ProductId && i.ColorId == stockUpsertVM.ColorId).ToList();
                foreach(var img in oldImgList)
                {
                    string oldImage = Path.Combine(wwwRootPath, img.Url.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImage))
                    {
                        System.IO.File.Delete(oldImage);

                    }
                }
                _context.SecondaryImages.RemoveRange(oldImgList);
                foreach (var item in secondaryImg)
                {

                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(item.FileName);
                    string filePath = Path.Combine(uploads, fileName + extension);
                    using (var fileStreams = new FileStream(filePath, FileMode.Create))
                    {
                        item.CopyTo(fileStreams);
                    }
                    ImageTool.Image_resize(filePath, filePath);

                    _context.SecondaryImages.Add(new SecondaryImage
                    {
                        ColorId = stockUpsertVM.ColorId,
                        ProductId = stockUpsertVM.ProductId,
                        Url = @"\img\products\" + fileName + extension
                    });
                }
            }





            _context.Stock.UpdateRange(stockQty);
            _context.SaveChanges();
            return RedirectToAction("Index", new { productId = stockUpsertVM.ProductId });
        }

    }
}
