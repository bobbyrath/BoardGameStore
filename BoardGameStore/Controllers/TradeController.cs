using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoardGameStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BoardGameStore.Controllers
{
    public class TradeController : Controller
    {
        private readonly BoardGameHubDbContext _context;
        private readonly UserManager<BoardGameHubUser> _boardGameHubUser;
        private Task<BoardGameHubUser> GetCurrentUserAsync() => _boardGameHubUser.GetUserAsync(HttpContext.User);


        public TradeController(BoardGameHubDbContext context, UserManager<BoardGameHubUser> boardGameHubUser)
        {
            _context = context;
            _boardGameHubUser = boardGameHubUser;
        }

        public IActionResult Index()
        {
            return View();
        }

        //Inject context in, pass in userID, display all based on that userID.

        public async Task<IActionResult> Inventory()
        {
            Inventory inventory = null;
            if (User.Identity.IsAuthenticated)
            {
                var currentUser = await _context.Users.Include(x => x.Inventory).ThenInclude(x => x.InventoryItems).FirstAsync(x => x.UserName == User.Identity.Name);
                inventory = currentUser.Inventory;
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            return View(inventory);
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Find()
        {
            var user = await GetCurrentUserAsync();
            var newView = _context.Users.Include(x => x.Inventory).ThenInclude(x => x.InventoryItems).Where(x => x.UserName != user.UserName);
            return View(newView.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> Create(InventoryItem newItem)
        {
            if (User.Identity.IsAuthenticated)
            {
                var currentUser = await _context.Users.Include(x => x.Inventory).ThenInclude(x => x.InventoryItems).FirstAsync(x => x.UserName == User.Identity.Name);
                if (currentUser.Inventory == null)
                {
                    currentUser.Inventory = new Inventory
                    {
                        UserID = currentUser.Id
                    };
                    _context.Add(currentUser.Inventory);
                }
                currentUser.Inventory.InventoryItems.Add(newItem);
                _context.SaveChanges();
            }
            return RedirectToAction("Inventory", "Trade");
        }
    }
}