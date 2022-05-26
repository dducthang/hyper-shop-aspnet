using HyperShop.DataAccess;
using HyperShop.Models;
using HyperShop.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace HyperShop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]

    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var items = _context.Carts.Where(c => c.UserId == userId).Include(c => c.Stock.Product).Include(c => c.Stock.Size)
                .Select(c=> new CartVM
                {
                    CartId = c.Id,
                    ProductName = c.Stock.Product.Name,
                    ProductImage = _context.PrimaryImages.FirstOrDefault(i=>i.ProductId == c.Stock.ProductId && i.ColorId==c.Stock.ColorId).Url,
                    Quantity = c.Quantity,
                    Price = c.Stock.Product.Price,
                    Size = c.Stock.Size.SizeValue,
                    StockQuantity = c.Stock.Quantity,
                }).ToList();
            return View(items);
        }


        #region API CALLS
        [HttpPost]
        [Authorize]
        public IActionResult AddToCart(string userId,int productId, int colorId, int sizeId, int quantity)
        {
            var stock = _context.Stock.FirstOrDefault(s => s.ProductId == productId && s.ColorId == colorId && s.SizeId == sizeId);
            if(stock != null)
            {
                var cartInDb = _context.Carts.FirstOrDefault(c => c.UserId == userId && c.StockId == stock.Id);
                if (cartInDb != null)
                {
                    cartInDb.Quantity += quantity;
                    
                }
                else
                {
                    var cart = new Cart()
                    {
                        UserId = userId,
                        StockId = stock.Id,
                        Quantity = quantity,
                    };
                    _context.Carts.Add(cart);   
                }
                _context.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }

        public IActionResult DeleteItem (int cartId)
        {
            var item = _context.Carts.FirstOrDefault(c => c.Id == cartId);
            if (item != null)
            {
                _context.Carts.Remove(item);
                _context.SaveChanges();
                return Ok();
            }
            return BadRequest();

        }
        public IActionResult Update()
        {
            var cartQty = HttpUtility.ParseQueryString(HttpContext.Request.QueryString.ToString());
            List<Cart> cartItems = _context.Carts.Where(c => c.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier))
                .Include(c => c.Stock.Product).Include(c => c.Stock.Size)
                .ToList();

            foreach (string cartIdString in cartQty.Keys)
            {
                int cartId = int.Parse(cartIdString);
                var item = cartItems.FirstOrDefault(c => c.Id == cartId);
                if (item != null)
                {
                    item.Quantity = int.Parse(cartQty[cartIdString]);
                }
            }
            _context.SaveChanges();
            List<CartVM> res = cartItems
                .Select(c => new CartVM
                {
                    CartId = c.Id,
                    ProductName = c.Stock.Product.Name,
                    ProductImage = _context.PrimaryImages.FirstOrDefault(i => i.ProductId == c.Stock.ProductId && i.ColorId == c.Stock.ColorId).Url,
                    Quantity = c.Quantity,
                    Price = c.Stock.Product.Price,
                    Size = c.Stock.Size.SizeValue,
                    StockQuantity = c.Stock.Quantity,
                }).ToList();
            return Ok(res);

        }


        #endregion
    }
}
