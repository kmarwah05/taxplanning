using System;
using System.Collections.Generic;

namespace tax_planning.Models
{
    public class Response
    {
        public Dictionary<DateTime, decimal> YearlyAmounts { get; set; }
        public Dictionary<DateTime, decimal> YearlyTax { get; set; }
        public decimal TotalCashOut { get; set; }
        public decimal TotalTaxPaid { get; set; }
    }
}
