﻿namespace Khata.Domain
{
    public class SalaryPayment : TrackedEntity, IWithdrawal
    {
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
        public decimal Amount { get; set; }
        public decimal BalanceBefore { get; set; }
        public decimal BalanceAfter => BalanceBefore - Amount;
        public string Description { get; set; }

        public string TableName => nameof(SalaryPayment);
        public int? RowId => Id;
    }
}