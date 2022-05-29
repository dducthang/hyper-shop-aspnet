using HyperShop.DataAccess;
using HyperShop.Models;
using HyperShop.Models.ViewModels;
using HyperShop.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
                    StockId = c.StockId
                }).ToList();
                return View(items);
            }
        }
        [Authorize]
        public IActionResult Checkout()
        {
            var user = _context.ApplicationUsers.First(u=>u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));

            Order order;
            string temp = HttpContext.Session.GetString("order");
            if (temp != null)
            {
                order = JsonConvert.DeserializeObject<Order>(temp);
            }
            else
            {
                order = new Order()
                {
                    Name = user.FullName,
                    Phone = user.PhoneNumber,
                    StreetAddress = user.StreetAddress,
                    District = user.District,
                    CityName = user.City
                
                };
            }
            ViewBag.Cities = _context.Cities.Select(s => new SelectListItem
            {
                Value = s.CityName,
                Text = s.CityName
            });
            return View(order);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (ModelState.IsValid)
            {
                HttpContext.Session.SetString("order", JsonConvert.SerializeObject(order));
                return RedirectToAction("Checkout2");
            }
            return View(order);
        }


        [Authorize]
        public IActionResult Checkout2()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orderString = HttpContext.Session.GetString("order");
            if (orderString == null) return BadRequest();
            var order = JsonConvert.DeserializeObject<Order>(orderString);
            ViewBag.ShipCost = _context.Cities.First(c => c.CityName == order.CityName).ShipCost;
            List<CartVM> cartItems = _context.Carts.Where(c => c.UserId == userId).Include(c => c.Stock.Product).Include(c => c.Stock.Size)
                .Select(c => new CartVM
                {
                    CartId = c.Id,
                    ProductName = c.Stock.Product.Name,
                    ProductImage = _context.PrimaryImages.FirstOrDefault(i => i.ProductId == c.Stock.ProductId && i.ColorId == c.Stock.ColorId).Url,
                    Quantity = c.Quantity,
                    Price = c.Stock.Product.Price,
                    Size = c.Stock.Size.SizeValue,
                    StockQuantity = c.Stock.Quantity,
                    StockId = c.StockId
                }).ToList();
            return View(cartItems);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Checkout2Post()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var orderString = HttpContext.Session.GetString("order");
            if (orderString == null) return BadRequest();
            var order = JsonConvert.DeserializeObject<Order>(orderString);
            order.ShipCost = _context.Cities.First(c => c.CityName == order.CityName).ShipCost;
            order.UserId = userId;
            order.Status = SD.Status_Prepare;
            _context.Orders.Add(order);
            _context.SaveChanges();
            List<Cart> itemsInCart = _context.Carts.Where(c => c.UserId == userId).ToList();
            List<OrderDetail> items = itemsInCart
                .Select(c => new OrderDetail()
                {
                    OrderId= order.Id,
                    StockId=c.StockId,
                    Quantity=c.Quantity,
                    Total = c.Price*c.Quantity,
                    PricePerUnit = c.Price
                })
                .ToList();
            double total = 0;
            List<int> invalidItems = new List<int>();
            for (int i = 0; i< items.Count;i++)
            {
                var itemsInStock = _context.Stock.First(s => s.Id == items[i].StockId);
                itemsInStock.Quantity -= items[i].Quantity;
                if (itemsInStock.Quantity<0)
                {
                    invalidItems.Add(i);
                }
                total += items[i].Total;
            }
            if(invalidItems.Count > 0)
            {
                string invalidItemsString = string.Join(", ", invalidItems);
                string nof = (invalidItems.Count > 1 ? "Items number"  : "Item number") + invalidItemsString + " in cart is not sufficient to serve";
                TempData["error"] = nof;

                //not update qty to stock
                _context.ChangeTracker.Clear();
                return RedirectToAction("Index", "Cart");
            }

            order.TotalPrice=total;
            _context.Orders.Update(order);

            _context.OrderDetails.AddRange(items);
            _context.Carts.RemoveRange(itemsInCart);
            _context.SaveChanges();
            HttpContext.Session.Remove("order");
            return RedirectToAction("Index", "Order");

        }

        #region API CALLS
        [HttpPost]
        public IActionResult AddToCart(int productId, int colorId, int sizeId, int quantity, double price)
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
                            Price = price
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

                foreach (string stockIdString in cartQty.Keys)
                {
                    int stokeId = int.Parse(stockIdString);
                    var item = cartItems.FirstOrDefault(c => c.StockId == stokeId && c.UserId == userId);
                    if (item != null)
                    {
                        item.Quantity = int.Parse(cartQty[stockIdString]);
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
                        StockId = c.StockId
                    }).ToList();
                return Ok(res);
            }

        }


        #endregion
    }
}
