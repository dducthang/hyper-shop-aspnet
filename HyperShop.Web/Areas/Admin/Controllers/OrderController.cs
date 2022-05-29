using HyperShop.DataAccess;
using HyperShop.Models.ViewModels;
using HyperShop.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HyperShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]


    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var orders = _context.Orders.ToList();
            return View(orders);
        }

        public IActionResult Detail(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return BadRequest();
            }
            var statuses = new List<string>()
            {
                SD.Status_Delivering,
                SD.Status_Receive,
                SD.Status_Prepare,
                SD.Status_Cancel,
            };
            ViewBag.Statuses = new SelectList(statuses);
            OrderVM items = new OrderVM()
            {

                OrderId = order.Id,
                Name = order.Name,
                StreetAddress = order.StreetAddress,
                District = order.District,
                City = order.CityName,
                Status = order.Status,
                OrderDate = order.OrderDate,
                Total = order.TotalPrice,
                ShipCost = order.ShipCost,
                Items = _context.OrderDetails.Where(d => d.OrderId == order.Id)
                    .Include(d => d.Stock.Product)
                    .Include(d => d.Stock.Size)
                    .Select(d => new OrderVmDetail()
                    {
                        ProductId = d.Stock.ProductId,
                        ProductName = d.Stock.Product.Name,
                        Size = d.Stock.Size.SizeValue,
                        Quantity = d.Quantity,
                        Total = d.Total,
                        PricePerUnit = d.PricePerUnit,
                        Image = _context.PrimaryImages.First(i => i.ColorId == d.Stock.ColorId && i.ProductId == d.Stock.ProductId).Url,
                    }).ToList()
            };
            return View(items);
        }
        [HttpPost]
        public IActionResult Detail(string Status, int OrderId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == OrderId);
            if (order == null) return BadRequest();
            order.Status = Status;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }

}
