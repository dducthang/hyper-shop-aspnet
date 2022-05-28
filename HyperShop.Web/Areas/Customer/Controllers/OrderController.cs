using HyperShop.DataAccess;
using HyperShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;

namespace HyperShop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = _context.Orders.Where(o => o.UserId == userId).ToList();
            return View(orders);
        }

        public IActionResult Detail(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = _context.Orders.FirstOrDefault(o => o.Id == id && o.UserId == userId);
            if(order == null)
            {
                return BadRequest();
            }
            OrderVM items = new OrderVM()
            {

                OrderId = order.Id,
                Name = order.Name,
                StreetAddress = order.StreetAddress,
                District = order.District,
                City= order.CityName,
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
                        Image = _context.PrimaryImages.First(i=>i.ColorId==d.Stock.ColorId && i.ProductId==d.Stock.ProductId).Url,
                    }).ToList()
            };
            return View(items);
        }


    }
}
