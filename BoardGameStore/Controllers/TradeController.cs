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
            var newUser =  _context.Users.Include(x => x.Inventory).ThenInclude(x => x.InventoryItems).Where(x => x.UserName != user.UserName);
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
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Inventory", "Trade");
        }

        public async Task<IActionResult> Propose(int id, string proposeeid)
        {
            ProposalViewModel model = new ProposalViewModel();
            var currentUser = await GetCurrentUserAsync();
            model.ProposeeItem = await _context.InventoryItems.FindAsync(id);
            model.Proposee = await _context.Users.Include(x => x.Inventory).ThenInclude(x => x.InventoryItems).FirstAsync(x => x.Id == proposeeid);
            model.Proposer = await _context.Users.Include(x => x.Inventory).ThenInclude(x => x.InventoryItems).FirstAsync(x => x.UserName == User.Identity.Name);
            return View(model);
        }

        //This should mark IsWanted to true. 
        [HttpPost]
        public async Task<IActionResult> Propose(ProposalViewModel model, int id)
        {
            Proposal proposal = new Proposal();
            var givingItem = await _context.InventoryItems.FindAsync(id);
            var wantedItem = await _context.InventoryItems.FindAsync(model.ProposeeItem.ID);
            givingItem.IsGiving = true;
            wantedItem.IsWanted = true;
            givingItem.IsTradeable = false;
            wantedItem.IsTradeable = false;
            proposal.Proposee = model.Proposee.UserName;
            proposal.ProposeeItem = wantedItem.Name;
            proposal.Proposer = model.Proposer.UserName;
            proposal.ProposerItem = givingItem.Name;
            await _context.Proposals.AddAsync(proposal);
            wantedItem.Proposal = proposal;
            givingItem.Proposal = proposal;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Proposal");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.InventoryItems.FindAsync(id);
            _context.InventoryItems.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction("Inventory");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.InventoryItems.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,IsTradeable, Inventory")] InventoryItem item)
        {
            _context.Update(item);
            await _context.SaveChangesAsync();
            return RedirectToAction("Inventory");
        }

        //Add proposer's item to proposee inventory, vice versa
        //Remove proposers items from their inventory, proposee's from theirs
        //Delete Proposal
        //Savechanges 
        [HttpPost]
        public async Task<IActionResult> Accept(int id)
        {
            var proposal = await _context.Proposals.FindAsync(id);
            var recipient = await _context.Users.Include(x => x.Inventory).ThenInclude(x => x.InventoryItems).FirstOrDefaultAsync(x => x.UserName == proposal.Proposee);
            var receivedItem = await _context.InventoryItems.FirstAsync(x => x.Proposal.ID == id && x.Name == proposal.ProposerItem);
            var sender = await _context.Users.Include(x => x.Inventory).ThenInclude(x => x.InventoryItems).FirstOrDefaultAsync(x => x.UserName == proposal.Proposer);
            var sentItem = await _context.InventoryItems.FirstAsync(x => x.Proposal.ID == id && x.Name == proposal.ProposeeItem);
            recipient.Inventory.InventoryItems.Add(receivedItem);
            recipient.Inventory.InventoryItems.Remove(sentItem);
            sender.Inventory.InventoryItems.Add(sentItem);
            sender.Inventory.InventoryItems.Remove(receivedItem);
            receivedItem.IsGiving = false;
            receivedItem.IsTradeable = false;
            sentItem.IsWanted = false;
            sentItem.IsTradeable = false;
            _context.Proposals.Remove(proposal);
            await _context.SaveChangesAsync();
            return View(proposal);
        }
    }
}