using Expense_Manager.Data;
using Expense_Manager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;

namespace Expense_Manager.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly AddDbContext _context;

        public TransactionsController(AddDbContext context)
        {
            _context = context;
        }

        //public async Task<IActionResult> Index()
        //{
        //    var transactions = await _context.Transactions.ToListAsync();
        //    var balance = transactions.Sum(t => t.Type == "Credit" ? t.Amount : -t.Amount);

        //    ViewBag.Balance = balance;
        //    return View(transactions);
        //}

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

        public async Task<IActionResult> Index(string search, string typeFilter)
        {
            var transactions = from t in _context.Transactions
                               select t;

            
            if (!string.IsNullOrEmpty(search))
            {
                transactions = transactions.Where(t => t.Description.Contains(search));
            }

           
            if (!string.IsNullOrEmpty(typeFilter))
            {
                transactions = transactions.Where(t => t.Type == typeFilter);
            }

            
            ViewBag.Balance = await transactions.SumAsync(t => t.Type == "Income" ? t.Amount : -t.Amount);

            return View(await transactions.ToListAsync());
        }


        public IActionResult DownloadPdf(string search, string typeFilter)
        {
           
            var transactions = _context.Transactions.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                transactions = transactions.Where(t => t.Description.Contains(search));
            }
            if (!string.IsNullOrEmpty(typeFilter))
            {
                transactions = transactions.Where(t => t.Type == typeFilter);
            }

            var list = transactions.ToList();


            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Header().Text("Transaction Report").FontSize(20).Bold().AlignCenter();
                    page.Content().Table(table =>
                    {
                        
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(100); 
                            columns.RelativeColumn();    
                            columns.RelativeColumn();    
                            columns.RelativeColumn();    
                        });

                        
                        table.Header(header =>
                        {
                            header.Cell().Text("Date").Bold();
                            header.Cell().Text("Type").Bold();
                            header.Cell().Text("Amount").Bold();
                            header.Cell().Text("Description").Bold();
                        });

                        
                        foreach (var t in list)
                        {
                            table.Cell().Text(t.Date.ToShortDateString());
                            table.Cell().Text(t.Type);
                            table.Cell().Text(t.Amount.ToString("C"));
                            table.Cell().Text(t.Description);
                        }
                    });
                });
            });

            var pdfBytes = document.GeneratePdf();
            return File(pdfBytes, "application/pdf", "TransactionsReport.pdf");
        }

    }
}
