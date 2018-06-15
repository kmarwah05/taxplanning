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

        public Table Schedule { get; set; }
        

        // Methods

        // Given desired additions, get withdrawal schedule
        public void CalculateSchedule()
        {
            Table table = new Table();

            var retirementLength = GetRetirementLength();
            var timeToRetirement = GetTimeToRetirement();

            // Present value
            var amount = GetFutureValueAfter(years: timeToRetirement, withAdditions: (Additions - CalculateTaxOnAddition(Additions)));

            // Do calculation
            (List<decimal> amount, decimal change) retirementSchedule = GetScheduleWith(amount, 0.00M, retirementLength);

            table.Additions = Additions;
            table.Withdrawal = retirementSchedule.change;
            table.AfterTaxWithdrawal = table.Withdrawal - CalculateTaxOnWithdrawal(table.Withdrawal);

            // Populate entire schedules
            List<decimal> amountForSchedule = new List<decimal>();

            for (var i = 0; i < timeToRetirement; i++)
            {
                amountForSchedule.Add(GetFutureValueAfter(years: i, withAdditions: Data.DesiredAdditions));
            }

            amountForSchedule.AddRange(retirementSchedule.amount);

            // Add schedules to the table
            table.YearlyAmount = amountForSchedule;

            Schedule = table;
        }

        protected virtual (List<decimal> amount, decimal change) GetScheduleWith(decimal initial, decimal final, int steps)
        {
            // Payment calculation
            var delta = (InterestRate * initial) / (1 - (decimal)Math.Pow(1 + (double)InterestRate, -steps));

            List<decimal> amount = new List<decimal>() { (decimal)initial };

            for (var i = 1; i < steps; i++)
            {
                amount.Add(CalculateNextYearAmount(amount[i - 1], delta));
            }

            return (amount, delta);
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

        protected int GetRetirementLength()
        {
            return Data.EndOfPlanDate - Data.RetirementDate;
        }

        protected int GetTimeToRetirement()
        {
            return Data.RetirementDate - DateTime.Today.Year;
        }

        protected abstract decimal CalculateTaxOnAddition(decimal addition);
        protected abstract decimal CalculateTaxOnWithdrawal(decimal withdrawal);
    }
}
