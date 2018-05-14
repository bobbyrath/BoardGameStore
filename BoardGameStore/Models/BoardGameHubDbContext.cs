using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }

    public class BoardGameHubUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

}
