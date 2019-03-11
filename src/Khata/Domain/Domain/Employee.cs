using System.Collections.Generic;

namespace Domain
{
    public class Employee : Person
    {
        public decimal Balance { get; set; }
        public string Designation { get; set; }
        public decimal Salary { get; set; }
        public string NIdNumber { get; set; }
        public virtual ICollection<SalaryIssue> SalaryIssues { get; set; }
        public virtual ICollection<SalaryPayment> SalaryPayments { get; set; }
    }
}
