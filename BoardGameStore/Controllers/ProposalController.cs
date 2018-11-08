using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoardGameStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace BoardGameStore.Controllers
{
    public class ProposalController : Controller
    {
        private BoardGameHubDbContext _context;
        public ProposalController(BoardGameHubDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var item = await _context.InventoryItems.Include(x => x.Proposal).FirstOrDefaultAsync(x => x.ID == id);
            var proposal = item.Proposal;
            
            return View(proposal);
        }

        public async Task<IActionResult> Remove(int id)
        {
            var proposal = _context.Proposals.Find(id);
            var proposerItem = await _context.InventoryItems.FirstAsync(x => x.Proposal.ID == id && x.Name == proposal.ProposerItem);
            var proposeeItem = await _context.InventoryItems.FirstAsync(x => x.Proposal.ID == id && x.Name == proposal.ProposeeItem);
            proposerItem.IsGiving = false;
            proposerItem.IsTradeable = true;
            proposeeItem.IsWanted = false;
            proposeeItem.IsTradeable = true;
            _context.Proposals.Remove(proposal);
            await _context.SaveChangesAsync();
            return View(proposal);
        }
    }
}