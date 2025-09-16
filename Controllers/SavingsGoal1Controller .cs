using Expense_Manager.Data;
using Expense_Manager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Expense_Manager.Controllers
{
    public class SavingsGoal1Controller : Controller
    {
        private readonly AddDbContext _context;

        public SavingsGoal1Controller(AddDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var goals = await _context.SavingsGoals.ToListAsync();
            return View(goals);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SavingsGoal goal)
        {
            if (ModelState.IsValid)
            {
                _context.SavingsGoals.Add(goal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(goal);
        }

        [HttpPost]
        public async Task<IActionResult> AddProgress(int id, decimal amount)
        {
            var goal = await _context.SavingsGoals.FindAsync(id);
            if (goal != null)
            {
                goal.SavedAmount += amount;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
