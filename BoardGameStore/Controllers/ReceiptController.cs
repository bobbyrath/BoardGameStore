using System;
using System.Linq;
using BoardGameStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SodaStore.Controllers
{
    public class ReceiptController : Controller
    {
        private BoardGameHubDbContext _context;
        public ReceiptController(BoardGameHubDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(Guid id)
        {
            return View(_context.Orders.Include(x => x.OrderItems).Single(x => x.ID == id));
        }
    }
}