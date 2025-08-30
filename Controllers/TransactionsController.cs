using Expense_Manager.Data;
using Expense_Manager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Expense_Manager.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly AddDbContext _context;

        public TransactionsController(AddDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var transactions = await _context.Transactions.ToListAsync();
            var balance = transactions.Sum(t => t.Type == "Credit" ? t.Amount : -t.Amount);

            ViewBag.Balance = balance;
            return View(transactions);
        }

        // ---------------- CREATE ----------------
        [HttpGet]
        public IActionResult Create()
        {
            var model = new Transaction { Date = DateTime.Today };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                await _context.Transactions.AddAsync(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(transaction);
        }

        // ---------------- EDIT ----------------
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
                return NotFound();

            return View(transaction);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Transaction transaction)
        {
            if (id != transaction.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(transaction);
        }

        // ---------------- DELETE ----------------
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
                return NotFound();

            return View(transaction);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
