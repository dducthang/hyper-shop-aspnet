using HyperShop.DataAccess;
using HyperShop.Models;
using HyperShop.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
            List<CartVM> items = new List<CartVM>();
            if (userId == null)
            {
                string temp = HttpContext.Session.GetString("cart");
                if(temp!= null)
                    items = JsonConvert.DeserializeObject<List<CartVM>>(temp);
                return View(items);
            }
            else
            {
                items = _context.Carts.Where(c => c.UserId == userId).Include(c => c.Stock.Product).Include(c => c.Stock.Size)
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
                return View(items);
            }
        }


        #region API CALLS
        [HttpPost]
        public IActionResult AddToCart(int productId, int colorId, int sizeId, int quantity)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Stock stock = _context.Stock.Where(s => s.ProductId == productId && s.ColorId == colorId && s.SizeId == sizeId)
                .Include(c => c.Product).Include(c => c.Size).FirstOrDefault();

            

            if(stock != null)
            {
                //anonymous
                if (userId == null)
                {
                    string temp = HttpContext.Session.GetString("cart");
                    var item = new CartVM()
                    {
                        ProductName = stock.Product.Name,
                        ProductImage = _context.PrimaryImages.FirstOrDefault(i => i.ProductId == stock.ProductId && i.ColorId == stock.ColorId).Url,
                        Quantity = quantity,
                        Price = stock.Product.Price,
                        Size = stock.Size.SizeValue,
                        StockQuantity = stock.Quantity,
                        StockId = stock.Id,
                    };
                    if (temp == null)
                    {
                        List<CartVM> cartList = new List<CartVM>()
                        {
                            item
                        };
                        HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cartList));

                    }
                    
                    
                    else
                    {
                        List<CartVM> cartList = JsonConvert.DeserializeObject<List<CartVM>>(temp);
                        var itemInCart = cartList.FirstOrDefault(c=>c.StockId == item.StockId);
                        if (itemInCart != null)
                        {
                            itemInCart.Quantity += quantity;

                        }
                        else
                        {
                            cartList.Add(item);
                        }
                        
                        HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cartList));

                    }

                    return Ok();
                }


                //signed in
                else
                {
                    var cartItemInDb = _context.Carts.FirstOrDefault(c => c.UserId == userId && c.StockId == stock.Id);
                    if (cartItemInDb != null)
                    {
                        cartItemInDb.Quantity += quantity;

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
            }
            return BadRequest();
        }

        public IActionResult DeleteItem (int stockId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                string temp = HttpContext.Session.GetString("cart");
                if (temp != null)
                {
                    List<CartVM> items = JsonConvert.DeserializeObject<List<CartVM>>(temp);
                    items.RemoveAll(i=>i.StockId==stockId);
                    HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(items));
                    return Ok();

                }

            }
            var item = _context.Carts.FirstOrDefault(c => c.StockId == stockId && c.UserId==userId);
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cartQty = HttpUtility.ParseQueryString(HttpContext.Request.QueryString.ToString());

            //for anonymous user
            if (userId == null)
            {
                string temp = HttpContext.Session.GetString("cart");
                if (temp != null)
                {
                    List<CartVM> items = JsonConvert.DeserializeObject<List<CartVM>>(temp);

                    foreach (string stokeIdString in cartQty.Keys)
                    {
                        int stokeId = int.Parse(stokeIdString);
                        var item = items.FirstOrDefault(c => c.StockId == stokeId);
                        if (item != null)
                        {
                            item.Quantity = int.Parse(cartQty[stokeIdString]);
                            if (item.Quantity == 0)
                            {
                                items.Remove(item);

                            }
                        }

                        
                    }
                    
                    HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(items));
                    return Ok(items);

                }
                return BadRequest();
            }
            //for signed in user
            else
            {
                List<Cart> cartItems = _context.Carts.Where(c => c.UserId == userId)
                .Include(c => c.Stock.Product).Include(c => c.Stock.Size)
                .ToList();

                foreach (string stokeIdString in cartQty.Keys)
                {
                    int stokeId = int.Parse(stokeIdString);
                    var item = cartItems.FirstOrDefault(c => c.StockId == stokeId && c.UserId == userId);
                    if (item != null)
                    {
                        item.Quantity = int.Parse(cartQty[stokeIdString]);
                        if (item.Quantity == 0)
                        {
                            _context.Carts.Remove(item);
                            cartItems.Remove(item);
                        }
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

        }


        #endregion
    }
}
