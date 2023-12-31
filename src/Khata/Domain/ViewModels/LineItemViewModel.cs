﻿
using Domain;

namespace ViewModels
{
    public class LineItemViewModel
    {
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public decimal NetPrice { get; set; }

        public int ItemId { get; set; }

        public LineItemType Type { get; set; }
    }
}