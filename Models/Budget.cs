namespace Expense_Manager.Models
{
    public class Budget
    {
        public int Id { get; set; }
        public string Category { get; set; }  
        public decimal PlannedAmount { get; set; }
        public int Month { get; set; } 
        public int Year { get; set; }   
    }
}
