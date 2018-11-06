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

        public IActionResult Details(int id)
        {
            var item = _context.InventoryItems.Include(x => x.Proposal).FirstOrDefault(x => x.ID == id);
            var proposal = item.Proposal;
            
            return View(proposal);
        }
    }
}