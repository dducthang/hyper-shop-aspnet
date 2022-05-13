﻿using System;
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

namespace HyperShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StockController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StockController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Stock?productId=3
        public IActionResult Index(int productId)
        {
            var stockList = _context.Stock
                .Where(s => s.ProductId == productId)
                .Include(s => s.Color)
                .ToList()
                .GroupBy(s => new { s.ColorId, s.Color.ColorValue })
                .Select(s => new Color
                {
                    Id = s.Key.ColorId,
                    ColorValue = s.Key.ColorValue
                });
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


            List<SizeQuantity> sizeQty2 = new List<SizeQuantity>();
            for (int i = 0; i < sizeQty.Count; i++)
            {
                sizeQty2.Add(new SizeQuantity
                {
                    Size = sizeQty[i].Size,
                    SizeId = sizeQty[i].SizeId,
                    Qty = sizeQty[i].Qty,

                });
            }

            StockUpsertVM stockUpsertVM = new StockUpsertVM
            {
                ProductId = productId,
                SizeQty = sizeQty2

            };
            ViewBag.Colors = _context.Colors.Select(s => new SelectListItem
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
        public IActionResult Create(StockUpsertVM stockUpsertVM)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            return View(stockUpsertVM);
        }

        // GET: Admin/Stock/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = await _context.Stock.FindAsync(id);
            if (stock == null)
            {
                return NotFound();
            }
            return View(stock);
        }

        // POST: Admin/Stock/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,SizeId,ColorId,Quantity")] Stock stock)
        {
            if (id != stock.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stock);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockExists(stock.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(stock);
        }

        // GET: Admin/Stock/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = await _context.Stock
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stock == null)
            {
                return NotFound();
            }

            return View(stock);
        }

        // POST: Admin/Stock/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stock = await _context.Stock.FindAsync(id);
            _context.Stock.Remove(stock);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StockExists(int id)
        {
            return _context.Stock.Any(e => e.Id == id);
        }
    }
}
