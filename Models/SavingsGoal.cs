using System.ComponentModel.DataAnnotations;

namespace Expense_Manager.Models
{
    public class SavingsGoal
    {
        public int Id { get; set; }

        [Required]
        public string GoalName { get; set; } = string.Empty;

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Target amount must be greater than 0")]
        public decimal TargetAmount { get; set; }

        public decimal SavedAmount { get; set; } = 0;

        [Required]
        public DateTime TargetDate { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
