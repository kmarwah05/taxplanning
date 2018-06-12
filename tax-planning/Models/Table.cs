using System;
using System.Collections.Generic;
using System.Linq;

namespace tax_planning.Models
{
    public class Table
    {
        public Dictionary<DateTime, decimal> YearlyAmounts { get; set; }
        public Dictionary<DateTime, decimal> YearlyTax { get; set; }
        public decimal TotalCashOut { get; set; }
        public decimal NetCashOut { get; set; }

        public static Table operator+ (Table a, Table b)
        {
            Table table = new Table();

            table.YearlyAmounts = new Dictionary<DateTime, decimal>();
            foreach (var key in a.YearlyAmounts.Keys)
            {
                table.YearlyAmounts[key] = a.YearlyAmounts[key] + b.YearlyAmounts[key];
            }

            table.YearlyTax = new Dictionary<DateTime, decimal>();
            foreach (var key in a.YearlyTax.Keys)
            {
                table.YearlyTax[key] = a.YearlyTax[key] + b.YearlyTax[key];
            }

            table.TotalCashOut = a.TotalCashOut + b.TotalCashOut;
            table.NetCashOut = a.NetCashOut + b.NetCashOut;

            return table;
        }
    }
}
