using System;
using BoardGameStore.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BoardGameStore
{
    internal static class DbInitializer
    {
        internal static void Initialize(this BoardGameHubDbContext db)
        {
           

            if (db.Products.Count() == 0)
            {
                db.Products.AddRange
                (
                    new Product { 
                        Description = "Farm game",
                        Name = "Agricola",
                        Price = 49.99m,
                        Image = "/images/agricola.jpg"
                    },
                    new Product
                    {
                        Description = "Party game",
                        Name = "Mysterium",
                        Price = 39.99m,
                        Image = "/images/mysterium.jpg"
                    },
                    new Product
                    {
                        Description = "Co-op game",
                        Name = "Betrayal At House On The Hill",
                        Price = 49.99m,
                        Image = "/images/betrayal.jpg"
                    }
                );
                db.SaveChanges();
            }
        }
    }
}