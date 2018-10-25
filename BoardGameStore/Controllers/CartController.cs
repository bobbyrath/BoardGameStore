using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoardGameStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BoardGameStore.Controllers
{
    public class CartController : Controller
    {
        private readonly BoardGameHubDbContext _context;

        public CartController(BoardGameHubDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            Cart myCart = null;
            if (User.Identity.IsAuthenticated)
            {
                var currentUser = _context.Users.Include(x => x.Cart).ThenInclude(x => x.CartItems).ThenInclude(x => x.Product).First(x => x.UserName == User.Identity.Name);
                if (currentUser.Cart != null)
                {
                    myCart = currentUser.Cart;
                }
                else if (Request.Cookies.ContainsKey("cartID"))
                {
                    if (Guid.TryParse(Request.Cookies["cartID"], out Guid cookieId))
                    {
                        myCart = _context.Carts.Include(x => x.CartItems).ThenInclude(x => x.Product).FirstOrDefault(x => x.CookieIdentifier == cookieId);
                    }
                }
            }
            else if (Request.Cookies.ContainsKey("cartID"))
            {
                if (Guid.TryParse(Request.Cookies["cartID"], out Guid cookieId))
                {
                    myCart = _context.Carts.Include(x => x.CartItems).ThenInclude(x => x.Product).FirstOrDefault(x => x.CookieIdentifier == cookieId);
                }
            }

            return View(myCart);

        }

        [HttpPost]
        public IActionResult Index(Cart model)
        {
            Cart myCart = null;
            if (User.Identity.IsAuthenticated)
            {
                var currentUser = _context.Users.Include(x => x.Cart).ThenInclude(x => x.CartItems).ThenInclude(x => x.Product).First(x => x.UserName == User.Identity.Name);
                if (currentUser.Cart != null)
                {
                    myCart = currentUser.Cart;
                }
            }
            else if (Request.Cookies.ContainsKey("cartID"))
            {
                if (Guid.TryParse(Request.Cookies["cartID"], out Guid cartID))
                {
                    myCart = _context.Carts.Include(x => x.CartItems).ThenInclude(x => x.Product).FirstOrDefault(x => x.CookieIdentifier == cartID);
                }
            }
            return View(myCart);
        }

        public IActionResult Remove(int id)
        {
            Guid cartID;
            Cart cart = null;
            if (Request.Cookies.ContainsKey("cartID"))
            {
                if (Guid.TryParse(Request.Cookies["cartID"], out cartID))
                {
                    cart = _context.Carts
                        .Include(carts => carts.CartItems)
                        .ThenInclude(cartitems => cartitems.Product)
                        .FirstOrDefault(x => x.CookieIdentifier == cartID);
                }
            }
            CartItem item = cart.CartItems.FirstOrDefault(x => x.ID == id);

            cart.LastModified = DateTime.Now;

            _context.CartItems.Remove(item);

            _context.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}