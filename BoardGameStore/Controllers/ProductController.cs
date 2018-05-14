using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BoardGameStore.Models;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace BoardGameStore.Controllers
{
    public class ProductController : Controller
    {
        //private List<Product> _products;
        private string _ProductConnectionString = null;


        public ProductController(IConfiguration config)
        {
            _ProductConnectionString = config.GetConnectionString("Product");
        }

        //_products = new List<Product>();
        //Image = "/images/Agricola-cards-in-play.jpg",
        //Image = "/images/mysterium.jpg",

        public IActionResult Index()
        {
            List<Product> _products = new List<Product>();
            using (SqlConnection connection = new SqlConnection(_ProductConnectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "spGetAllProducts";
                    command.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        int idColumn = reader.GetOrdinal("ID");
                        int nameColumn = reader.GetOrdinal("ProductName");
                        int descriptionColumn = reader.GetOrdinal("Description");
                        int imageColumn = reader.GetOrdinal("Image");
                        while (reader.Read())
                        {
                            int productModelID = reader.GetInt32(idColumn);
                            string name = reader.GetString(nameColumn);
                            string description = reader.GetString(descriptionColumn);
                            string image = reader.GetString(imageColumn);
                            _products.Add(new Product
                            {
                                ID = productModelID,
                                Description = description,
                                Name = name,
                                Image = image
                            });
                        }
                    }
                }

                //foreach (var product in _products)
                //{
                //    using (SqlCommand imageCommand = connection.CreateCommand())
                //    {
                //        imageCommand.CommandText = "spGetProductImages";
                //        imageCommand.CommandType = CommandType.StoredProcedure;
                //        imageCommand.Parameters.AddWithValue("@productModelID", product.ID);
                //        using (SqlDataReader reader2 = imageCommand.ExecuteReader())
                //        {
                //            while (reader2.Read())
                //            {
                //                byte[] imageBytes = (byte[])reader2[0];
                //                product.Image = "data:image/jpeg;base64, " + Convert.ToBase64String(imageBytes);
                //                break;
                //            }
                //        }
                //    }
                //}
            }

            return View(_products);
        }

        public IActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                
            }
            return NotFound();
        }
    }
}