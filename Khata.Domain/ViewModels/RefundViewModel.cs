﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Khata.ViewModels
{
    public class RefundViewModel
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int SaleId { get; set; }

        public ICollection<LineItemViewModel> Cart { get; set; }


        [DataType(DataType.Currency)]
        [Display(Name = "Cash Back")]
        public decimal CashBack { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Debt Rollback")]
        public decimal DebtRollback { get; set; }

        public string Description { get; set; }
    }
}
