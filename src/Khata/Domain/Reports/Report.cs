using System;

namespace Domain.Reports
{
    public class Report { }

    public class IndividualReport : Report
    {
        public int Id { get; set; }
    }

    public class PeriodicalReport<TReport> 
        : Report where TReport: Report
    {
        public DateTime ReportDate { get; set; }

        public TReport Daily { get; set; }
        public TReport Weekly { get; set; }
        public TReport Monthly { get; set; }
    }
}
