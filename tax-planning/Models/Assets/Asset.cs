using System;
using System.Collections.Generic;

namespace tax_planning.Models
{
    public abstract class Asset
    {
        // Properties
        public string Name { get; set; }

        public string AssetType { get; set; }

        public decimal Value { get; set; }

        public decimal InterestRate { get; set; } = 0.06M;

        public decimal Additions { get; set; }

        public bool Preferred { get; set; }

        public Table Schedule { get; set; }
        

        // Methods

        // Given desired additions, get withdrawal schedule
        public void CalculateSchedule()
        {
            Table table = new Table();

            var retirementLength = Data.EndOfPlanDate - Data.RetirementDate;
            var timeToRetirement = Data.RetirementDate - DateTime.Today.Year;

            List<decimal> amounts = new List<decimal>();

            // Add additions up to retirement
            for (var i = 0; i < timeToRetirement; i++)
            {
                amounts.Add(GetFutureValueAfter(years: i, withAdditions: (Additions - CalculateTaxOnAddition(Additions))));
            }

            // Get the withdrawal
            var delta = GetWithdrawalFor(amounts[timeToRetirement - 1], retirementLength);

            // Populate the rest of the schedule
            for (var i = timeToRetirement; i < retirementLength + timeToRetirement; i++)
            {
                amounts.Add(CalculateNextYearAmount(amounts[i - 1], -delta));
            }

            table.Withdrawal = delta;
            table.AfterTaxWithdrawal = table.Withdrawal - CalculateTaxOnWithdrawal(table.Withdrawal);
            table.YearlyAmount = amounts;
            table.TotalCashOut = table.AfterTaxWithdrawal * retirementLength;
            table.NetCashOut = table.TotalCashOut - Additions;

            Schedule = table;
        }

        protected virtual decimal GetWithdrawalFor(decimal principal, int steps)
        {
            // Payment calculation
            return (InterestRate * principal) / (1 - (decimal)Math.Pow(1 + (double)InterestRate, -steps));
        }

        protected decimal GetFutureValueAfter(int years, decimal withAdditions = 0.00M)
        {
            var futureValue = Value;
            for (var i = 0; i < years; i++)
            {
                futureValue = CalculateNextYearAmount(previousYearAmount: futureValue, yearDelta: withAdditions);
            }
            return futureValue;
        }

        protected virtual decimal CalculateNextYearAmount(decimal previousYearAmount, decimal yearDelta)
        {
            return previousYearAmount * (InterestRate + 1.00M) + yearDelta;
        }

        protected abstract decimal CalculateTaxOnAddition(decimal addition);
        protected abstract decimal CalculateTaxOnWithdrawal(decimal withdrawal);


        // Data storage class
        public class Table
        {
            public List<int> Years
            {
                get
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
}
