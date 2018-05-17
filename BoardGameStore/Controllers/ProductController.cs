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
    //public class ProductController : Controller
    //{

    //    //private string _ProductConnectionString = null;
    //    private readonly BoardGameHubDbContext _context;

    //    public ProductController(BoardGameHubDbContext context)
    //    {
    //        _context = context;
    //    }
    //    //public ProductController(IConfiguration config)
    //    //{
    //    //    _ProductConnectionString = config.GetConnectionString("Product");
    //    //}

    //    public IActionResult Index()
    //    {
    //        List<Product> products = _context.Products.ToList();
    //        return View(products);
    //    }

    //    //ADO.NET
    //    //List<Product> _products = new List<Product>();
    //    //using (SqlConnection connection = new SqlConnection(_ProductConnectionString))
    //    //{
    //    //    connection.Open();
    //    //    using (SqlCommand command = connection.CreateCommand())
    //    //    {
    //    //        command.CommandText = "spGetAllProducts";
    //    //        command.CommandType = CommandType.StoredProcedure;
    //    //        using (SqlDataReader reader = command.ExecuteReader())
    //    //        {
    //    //            int idColumn = reader.GetOrdinal("ID");
    //    //            int nameColumn = reader.GetOrdinal("ProductName");
    //    //            int descriptionColumn = reader.GetOrdinal("Description");
    //    //            int imageColumn = reader.GetOrdinal("Image");
    //    //            while (reader.Read())
    //    //            {
    //    //                int productModelID = reader.GetInt32(idColumn);
    //    //                string name = reader.GetString(nameColumn);
    //    //                string description = reader.GetString(descriptionColumn);
    //    //                string image = reader.GetString(imageColumn);
    //    //                _products.Add(new Product
    //    //                {
    //    //                    ID = productModelID,
    //    //                    Description = description,
    //    //                    Name = name,
    //    //                    Image = image
    //    //                });
    //    //            }
    //    //        }
    //    //    }

    //    public IActionResult Details(int? id)
    //    {
    //        if (id.HasValue)
    //        {
    //            Product p = _context.Products.Find(id.Value);
    //            return View(p);
    //        }
    //        return NotFound();
    //    }

    //    [HttpPost]
    //    public IActionResult Details(int id, int quantity = 1)
    //    {
    //        Guid cartId;
    //        Cart cart = null;
    //        if (Request.Cookies.ContainsKey("cartID"))
    //        {
    //            if(Guid.TryParse(Request.Cookies["cartId"], out cartId))
    //            {
    //                cart = _context.Carts
    //                    .Include(carts => carts.CartItems)
    //                    .ThenInclude(cartitems => cartitems.Product)
    //                    .FirstOrDefault(x => x.CookieIdentifier == cartId);
    //            }
    //        }

    //        if (cart == null)
    //        {
    //            cart = new Cart();
    //            cartId = Guid.NewGuid();
    //            cart.CookieIdentifier = cartId;

    //            _context.Carts.Add(cart);
    //            Response.Cookies.Append("cartId", cartId.ToString(), new Microsoft.AspNetCore.Http.CookieOptions { Expires = DateTime.UtcNow.AddYears(100) });
    //        }

    //        CartItem item = cart.CartItems.FirstOrDefault(x => x.Product.ID == id);
    //        if (item == null)
    //        {
    //            item = new CartItem();
    //            item.Product = _context.Products.Find(id);
    //            cart.CartItems.Add(item);
    //        }

    //        item.Quantity += quantity; 
    //        cart.LastModified = DateTime.Now;

    //        _context.SaveChanges();
    //        return RedirectToAction("Index", "Cart");
    //    }
    //}

    public class ProductController : Controller
    {
        private readonly BoardGameHubDbContext _context;

        public ProductController(BoardGameHubDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            List<Product> products = _context.Products.ToList();
            return View(products);
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