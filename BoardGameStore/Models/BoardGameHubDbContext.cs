using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BoardGameStore.Models
{
    public class BoardGameHubDbContext : IdentityDbContext<BoardGameHubUser>
    {
        public BoardGameHubDbContext(): base()
        {

        }

        public BoardGameHubDbContext(DbContextOptions options): base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems{ get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems{ get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
    }

    public class BoardGameHubUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Cart Cart { get; set; }
    }

    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //This type is nullable - I don't want price equal to 0
        public decimal? Price { get; set; }
        public string Image { get; set; }
        public string Category { get; set; }
    }

    public class Cart
    {
        public Cart()
        {
            this.CartItems = new HashSet<CartItem>();

        }

        public int ID { get; set; }
        public Guid CookieIdentifier { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastModified { get; set; }
        
    }

    public class CartItem
    {
        public int ID { get; set; }
        public Cart Cart { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastModified { get; set; }
    }

    public class Order
    {
        public Order()
        {
            this.OrderItems = new HashSet<OrderItem>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid ID { get; set; }
        public string ContactEmail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ShippingStreet { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingState { get; set; }
        public string ShippingPostalCode { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastModified { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }

    public class OrderItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int? ProductID { get; set; }
        public string Description { get; set; }
        //This type is nullable - I don't want price equal to 0
        public decimal? Price { get; set; }
        public string Category { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastModified { get; set; }
        public Order Order { get; set; }
    }

    public class Inventory
    {
        public int ID { get; set; }
        public BoardGameHubUser User { get; set; }
        public ICollection<InventoryItem> InventoryItems { get; set; }
    }

    public class InventoryItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsTradeable { get; set; }
        public Inventory Inventory { get; set; }
    }
}
