using System;
using System.Collections.Generic;
using System.Linq;

namespace tax_planning.Models
{
    public class Table
    {
        public List<int> Years { get
            {
                List<int> list = new List<int>();

                for (var i = 0; i < YearlyAmount.Count; i++)
                {
                    list.Add(DateTime.Today.Year + i);
                }

                return list;
            }
        }

        public List<decimal> YearlyAmount { get; set; }

        public List<decimal> YearlyChange { get; set; }

        public decimal TotalCashOut => YearlyChange.Aggregate(0.00M, (sum, next) => next < 0M ? sum - next : sum);

        public decimal NetCashOut => TotalCashOut
            - YearlyChange.Aggregate(0.00M, (sum, next) => next > 0M ? sum + next : sum);

        // Extension method allowing you to add two tables together
        public static Table operator+ (Table a, Table b)
        {
            Table table = new Table();

            table.YearlyAmount = new List<decimal>();

            table.YearlyChange = new List<decimal>();
            for (var i = 0; i < a.YearlyChange.Count; i++)
            {
                table.YearlyChange.Add(a.YearlyChange[i] + b.YearlyChange[i]);
                table.YearlyAmount.Add(a.YearlyAmount[i] + b.YearlyAmount[i]);
            }

            return table;
        }
    }
}
