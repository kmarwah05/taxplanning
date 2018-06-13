using System.Collections.Generic;
using System.Linq;

namespace tax_planning.Models
{
    public class Table
    {
        public List<decimal> YearlyAmounts { get; set; }

        public List<decimal> YearlyTax { get; set; }

        public decimal TotalCashOut => YearlyAmounts.Aggregate(0.00M, (sum, next) => next < 0M ? sum - next : sum);

        public decimal NetCashOut => TotalCashOut
            - YearlyAmounts.Aggregate(0.00M, (sum, next) => next > 0M ? sum + next : sum)
            - YearlyTax.Aggregate(0.00M, (sum, next) => sum + next);

        // Extension method allowing you to add two tables together
        public static Table operator+ (Table a, Table b)
        {
            Table table = new Table();

            table.YearlyAmounts = new List<decimal>();

            table.YearlyTax = new List<decimal>();
            for (var i = 0; i < a.YearlyTax.Count; i++)
            {
                table.YearlyTax[i] = a.YearlyTax[i] + b.YearlyTax[i];
                table.YearlyAmounts[i] = a.YearlyAmounts[i] + b.YearlyAmounts[i];
            }

            return table;
        }
    }
}
