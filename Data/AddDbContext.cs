using System.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Expense_Manager.Data
{
    public class AddDbContext : DbContext
    {
        public AddDbContext(DbContextOptions<AddDbContext> options):base(options)
        {
            
        }

        public DbSet<Expense_Manager.Models.Transaction> Transactions { get; set; }


    }
}
