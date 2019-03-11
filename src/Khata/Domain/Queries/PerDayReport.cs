using System;

namespace Queries
{
    public class PerDayReport : Report
    {
        public DateTime Date { get; set; }

        public decimal CashIn { get; set; }
        public decimal CashOut { get; set; }
        public decimal NewReceivable { get; set; }
        public decimal NewPayable { get; set; }
    }
}
