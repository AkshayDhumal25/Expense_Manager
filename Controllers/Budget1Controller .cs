using Expense_Manager.Data;
using Expense_Manager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Expense_Manager.Controllers
{
    public class Budget1Controller : Controller
    {
        private readonly AddDbContext _context;
        public Budget1Controller(AddDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var budgets = await _context.Budgets
                .OrderByDescending(b => b.Year).ThenByDescending(b => b.Month)
                .ToListAsync();
            return View(budgets);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Budget budget)
        {
            if (ModelState.IsValid)
            {
                _context.Budgets.Add(budget);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(budget);
        }
    }
}
