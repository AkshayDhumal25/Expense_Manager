﻿namespace Expense_Manager.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public int Amount { get; set; }

        public string Type { get; set; } 

        public string Description { get; set; }

        public DateTime Date { get; set; }
    }
}
