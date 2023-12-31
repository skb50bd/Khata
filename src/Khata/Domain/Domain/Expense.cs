﻿namespace Domain
{
    public class Expense : TrackedDocument, IWithdrawal
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }

        public string TableName => nameof(Expense);
        public int? RowId => Id;
    }
}