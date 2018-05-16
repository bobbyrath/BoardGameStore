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
            db.Database.Migrate();

            if (db.Products.Count() == 0)
            {
                db.Products.Add(new Product
                {
                    Description = "Farm game",
                    Name = "Agricola",
                    Price = 49.99m,
                    Image = "/images/agricola.jpg"
                });

                db.SaveChanges();
            }
        }
    }
}