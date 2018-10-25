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

        public IActionResult Index()
        {
            CheckoutViewModel model = new CheckoutViewModel();
            if (User.Identity.IsAuthenticated)
            {
                var currentUser = _context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
                model.ContactEmail = currentUser.Email;
                model.FirstName = currentUser.FirstName;
                model.LastName = currentUser.LastName;
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(CheckoutViewModel model)
        {
            if (ModelState.IsValid)
            {
                // TODO: Do some more advanced validation 
                //  - the address info is required, but is it real? I can use an API to find out!
                //  - the credit card is required, but does it have available funds?  Again, I can use an API

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
                if (myCart == null)
                {
                    ModelState.AddModelError("Cart", "There was a problem with your cart, please check your cart to verify that all items are correct");
                }
                else
                {
                    // Take the existing cart, and convert the cart and cart items to an  "order" with "order items"
                    //  - when creating order items, I'm going to "denormalize" the info to copy the price, description, etc. of what the customer ordered.
                    Order order = new Order
                    {
                        ContactEmail = model.ContactEmail,
                        Created = DateTime.UtcNow,
                        FirstName = model.FirstName,
                        LastModified = DateTime.UtcNow,
                        LastName = model.LastName,
                        ShippingCity = model.ShippingCity,
                        ShippingPostalCode = model.ShippingPostalCode,
                        ShippingState = model.ShippingState,
                        ShippingStreet = model.ShippingStreet,
                        OrderItems = myCart.CartItems.Select(x => new OrderItem
                        {
                            Created = DateTime.UtcNow,
                            LastModified = DateTime.UtcNow,
                            Description = x.Product.Description,
                            ProductID = x.Product.ID,
                            Name = x.Product.Name,
                            Price = x.Product.Price,
                            Quantity = x.Quantity
                        }).ToHashSet()
                    };

                    _context.Orders.Add(order);
                    // Delete the cart, cart items, and clear the cookie or "user cart" info so that the user will get a new cart next time.
                    _context.Carts.Remove(myCart);

                    if (User.Identity.IsAuthenticated)
                    {
                        var currentUser = _context.Users.Include(x => x.Cart).ThenInclude(x => x.CartItems).ThenInclude(x => x.Product).First(x => x.UserName == User.Identity.Name);
                        currentUser.Cart = null;
                    }
                    Response.Cookies.Delete("cartID");

                    _context.SaveChanges();


                    // TODO: Email the user to let them know their order has been placed. -- I need an API for this!

                    // Redirect to the receipt page
                    return RedirectToAction("Index", "Receipt", new { ID = order.ID });
                }
            }
            return View(model);
        }
    }
}