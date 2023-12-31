﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Domain;

namespace DTOs
{
    public class SaleDto
    {
        public int Id { get; set; }
        public bool IsRemoved { get; set; }

        public int OutletId { get; set; }
        public virtual OutletDto Outlet { get; set; }

        [Display(Name = "Type")]
        public SaleType Type { get; set; }

        [Display(Name = "Sale Date")]
        [DataType(DataType.Date)]
        public DateTimeOffset SaleDate { get; set; }

        public int InvoiceId { get; set; }
        public virtual CustomerInvoice Invoice { get; set; }

        public int CustomerId { get; set; }
        public CustomerDto Customer { get; set; }

        public ICollection<SaleLineItem> Cart { get; set; }

        [Display(Name = "Subtotal")]
        [DataType(DataType.Currency)]
        public decimal PaymentSubTotal { get; set; }

        [Display(Name = "Discount")]
        [DataType(DataType.Currency)]
        public decimal PaymentDiscountCash { get; set; }

        [Display(Name = "Discount (%)")]
        public float PaymentDiscountPercentage { get; set; }

        [Display(Name = "Total")]
        [DataType(DataType.Currency)]
        public decimal PaymentTotal { get; set; }

        [Display(Name = "Paid")]
        [DataType(DataType.Currency)]
        public decimal PaymentPaid { get; set; }

        [Display(Name = "Due")]
        [DataType(DataType.Currency)]
        public decimal PaymentDue { get; set; }

        [Display(Name = "Profit")]
        [DataType(DataType.Currency)]
        public decimal Profit { get; set; }

        public string Description { get; set; }

        public Metadata Metadata { get; set; }
    }
}