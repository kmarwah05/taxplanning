using System;
using System.Collections.Generic;

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

        public decimal Additions { get; set; }

        public decimal Withdrawal { get; set; }

        public decimal AfterTaxWithdrawal { get; set; }

        public decimal TotalCashOut { get; set; }

        public decimal NetCashOut { get; set; }

        public Table()
        {
            YearlyAmount = new List<decimal>();
        }

        public Table(int length)
        {
            YearlyAmount = new List<decimal>();
            YearlyAmount.AddRange(new decimal[length]);
        }
    }
}
