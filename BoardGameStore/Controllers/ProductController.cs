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
      
        //private string _ProductConnectionString = null;
        private readonly BoardGameHubDbContext _context;

        public ProductController(BoardGameHubDbContext context)
        {
            _context = context;
        }
        //public ProductController(IConfiguration config)
        //{
        //    _ProductConnectionString = config.GetConnectionString("Product");
        //}

        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }
        
            //ADO.NET
            //List<Product> _products = new List<Product>();
            //using (SqlConnection connection = new SqlConnection(_ProductConnectionString))
            //{
            //    connection.Open();
            //    using (SqlCommand command = connection.CreateCommand())
            //    {
            //        command.CommandText = "spGetAllProducts";
            //        command.CommandType = CommandType.StoredProcedure;
            //        using (SqlDataReader reader = command.ExecuteReader())
            //        {
            //            int idColumn = reader.GetOrdinal("ID");
            //            int nameColumn = reader.GetOrdinal("ProductName");
            //            int descriptionColumn = reader.GetOrdinal("Description");
            //            int imageColumn = reader.GetOrdinal("Image");
            //            while (reader.Read())
            //            {
            //                int productModelID = reader.GetInt32(idColumn);
            //                string name = reader.GetString(nameColumn);
            //                string description = reader.GetString(descriptionColumn);
            //                string image = reader.GetString(imageColumn);
            //                _products.Add(new Product
            //                {
            //                    ID = productModelID,
            //                    Description = description,
            //                    Name = name,
            //                    Image = image
            //                });
            //            }
            //        }
            //    }

        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .SingleOrDefaultAsync(m => m.ID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }
}