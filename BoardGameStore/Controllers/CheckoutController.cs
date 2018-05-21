using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BoardGameStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BoardGameStore.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly BoardGameHubDbContext _context;

        public CheckoutController(BoardGameHubDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(int id)
        {
            Guid cartId;
            Cart cart = null;
            if (Request.Cookies.ContainsKey("cartId"))
            {
                if (Guid.TryParse(Request.Cookies["cartId"], out cartId))
                {
                    //https://docs.microsoft.com/en-us/ef/core/querying/related-data
                    cart = _context.Carts
                        .Include(carts => carts.CartItems)
                        .ThenInclude(cartitems => cartitems.Product)
                        .FirstOrDefault(x => x.CookieIdentifier == cartId);
                }
            }

            if (cart == null)
            {
                cart = new Cart();
                cartId = Guid.NewGuid();
                cart.CookieIdentifier = cartId;

                _context.Carts.Add(cart);
                Response.Cookies.Append("cartId", cartId.ToString(), new Microsoft.AspNetCore.Http.CookieOptions { Expires = DateTime.UtcNow.AddYears(100) });

            }
            CartItem item = cart.CartItems.FirstOrDefault(x => x.Product.ID == id);
            if (item == null)
            {
                item = new CartItem();
                item.Product = _context.Products.Find(id);
                cart.CartItems.Add(item);
            }

            
            cart.LastModified = DateTime.Now;
            Order order = new Order();
            OrderItem orderitem = order.OrderItems.FirstOrDefault(x => x.CartItem.ID == id);
            if (orderitem == null)
            {
                orderitem = new OrderItem();
                orderitem.CartItem = _context.CartItems.Find(id);
                order.OrderItems.Add(orderitem);
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Order");
        }
    }
}