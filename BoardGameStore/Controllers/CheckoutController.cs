using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BoardGameStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace BoardGameStore.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly BoardGameHubDbContext _context;
        private SignInManager<BoardGameHubUser> _signInManager;

        public CheckoutController(BoardGameHubDbContext context)
        {
            _context = context;
        }


        private async Task GetCurrentCart(CheckoutViewModel model)
        {
            Guid cartId;
            Cart cart = null;
            if (User.Identity.IsAuthenticated)
            {
                var currentUser = await _signInManager.UserManager.GetUserAsync(User);
                model.Email = currentUser.Email;
                model.FirstName = currentUser.FirstName;
                model.LastName = currentUser.LastName;
            }

            if (Request.Cookies.ContainsKey("cartId"))
            {
                if (Guid.TryParse(Request.Cookies["cartId"], out cartId))
                {
                    cart = await _context.Carts
                        .Include(carts => carts.CartItems)
                        .ThenInclude(cartitems => cartitems.Product)
                        .FirstOrDefaultAsync(x => x.CookieIdentifier == cartId);
                }
            }
            model.Cart = cart;
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Index(CheckoutViewModel model)
        {
            await GetCurrentCart(model);
            if (ModelState.IsValid)
            {
                Order newOrder = new Order
                {
                    OrderItems = model.Cart.CartItems.Select(x => new OrderItem
                    {
                        ProductID = x.Product.ID,
                        ProductName = x.Product.Name,
                        ProductPrice = x.Product.Price,
                        Quantity = x.Quantity
                    }).ToArray()
                };
                _context.Orders.Add(newOrder);
                _context.CartItems.RemoveRange(model.Cart.CartItems);
                _context.Carts.Remove(model.Cart);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Order");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}