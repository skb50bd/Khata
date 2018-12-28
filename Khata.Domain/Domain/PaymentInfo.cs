﻿namespace Khata.Domain
{
    public class PaymentInfo
    {
        public decimal SubTotal { get; set; }
        public decimal DiscountCash { get; set; }
        public float DiscountPercentage => (float)(DiscountCash / SubTotal * 100M);
        public decimal Total => SubTotal - DiscountCash;
        public decimal Paid { get; set; }
        public decimal Due => Total - Paid;
    }
}