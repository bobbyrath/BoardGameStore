using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BoardGameStore.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace BoardGameStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly BoardGameHubDbContext _context;

        public ProductController(BoardGameHubDbContext context)
        {
            _context = context;
        }

        public IActionResult Empty()
        {
            return View();
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var games = from m in _context.Products
                        select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                games = games.Where(s => s.Name.Contains(searchString));
                if (games.ToList().Count == 0)
                {
                    return RedirectToAction("Empty", "Product");
                }
            }
            return View(await games.ToListAsync());
        }


        public IActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                Product p = _context.Products.Find(id.Value);
                return View(p);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Details(int id, int quantity = 1)
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

            item.Quantity += quantity;
            cart.LastModified = DateTime.Now;

            _context.SaveChanges();
            return RedirectToAction("Index", "Cart");
        }
    }
}