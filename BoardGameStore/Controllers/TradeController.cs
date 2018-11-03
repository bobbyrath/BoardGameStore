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
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var user = await GetCurrentUserAsync();
            var newUser = _context.Users.Include(x => x.Inventory).ThenInclude(x => x.InventoryItems).Where(x => x.UserName != user.UserName);
            return View(newUser.ToList());
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

        public IActionResult Propose(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = _context.InventoryItems.FirstOrDefault(m => m.ID == id);
            if (game == null)
            {
                return NotFound();
            }
            return View(game);
        }

        
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.InventoryItems.FindAsync(id);
            _context.InventoryItems.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction("Inventory");
        }
    }
}