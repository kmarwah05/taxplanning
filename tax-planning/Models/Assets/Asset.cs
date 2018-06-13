using System;
using System.Collections.Generic;

namespace tax_planning.Models
{
    public abstract class Asset
    {
        // Properties
        public string Name { get; set; }

        public decimal Value { get; set; }

        public decimal InterestRate { get; set; } = 0.06M;

        // Methods

        // Given desired additions, get withdrawal schedule
        public virtual Table GetOptimalScheduleFor(Data data)
        {
            Table table = new Table();

            var retirementLength = GetRetirementLength(data);
            var timeToRetirement = GetTimeToRetirement(data);
            var additions = data.DesiredAdditions;

            // Rough estimate to start, quality of estimation increases speed of Newton-Raphson
            var withdrawal = (additions * timeToRetirement + Value) / retirementLength;

            // Initial Condition
            var amount = GetFutureValueAfter(years: timeToRetirement);

            // Do calculation
            (List<decimal> amount, List<decimal> change) retirementSchedule = GetScheduleWith((float)withdrawal, amount, 0.00M, retirementLength);

            // Populate entire schedules
            List<decimal> amountForSchedule = new List<decimal>();
            List<decimal> changeForSchedule = new List<decimal>();

            for (var i = 0; i < timeToRetirement; i++)
            {
                amountForSchedule.Add(GetFutureValueAfter(years: i, withAdditions: data.DesiredAdditions));
                changeForSchedule.Add(data.DesiredAdditions);
            }

            amountForSchedule.AddRange(retirementSchedule.amount);
            changeForSchedule.AddRange(retirementSchedule.change);

            // Add scehdules to the table
            table.YearlyAmount = amountForSchedule;
            table.YearlyChange = changeForSchedule;

            return table;
        }

        // Given desired withdrawal, get addition schedule
        public virtual Table GetDesiredScheduleFor(Data data)
        {
            Table table = new Table();

            var retirementLength = GetRetirementLength(data);
            var timeToRetirement = GetTimeToRetirement(data);
            var withdrawal = data.DesiredWithdrawalAmount;

            // End condition
            var peak = Value;

            for (var i = 0; i < retirementLength; i++)
            {
                peak = (peak + withdrawal) / (InterestRate + 1.00M);
            }

            // Guess
            decimal additions = (peak - Value) / timeToRetirement;

            // Do calculation, gives partial schedule
            (List<decimal> amount, List<decimal> change) preRetirementSchedule = GetScheduleWith((float)additions, Value, peak, retirementLength);

            // Populate entire schedules
            List<decimal> amountForSchedule = new List<decimal>();
            List<decimal> changeForSchedule = new List<decimal>();

            amountForSchedule.AddRange(preRetirementSchedule.amount);
            changeForSchedule.AddRange(preRetirementSchedule.change);

            for (var i = 0; i < retirementLength; i++)
            {
                amountForSchedule.Add(GetFutureValueAfter(years: i, startingFrom: peak, withAdditions: data.DesiredWithdrawalAmount));
                changeForSchedule.Add(data.DesiredWithdrawalAmount);
            }

            // Add schedules to the table
            table.YearlyAmount = amountForSchedule;
            table.YearlyChange = changeForSchedule;

            return table;
        }

        protected virtual (List<decimal> amount, List<decimal> changes) GetScheduleWith(float delta, decimal initial, decimal final, int steps)
        {
            
            List<decimal> amount = new List<decimal>() { initial };
            List<decimal> change = new List<decimal>();

            // Amount at final, want f(x) = 0
            float f(float x)
            {
                var sum = 0.0f;
                for (var i = 0; i < steps - 1; i++)
                {
                    sum += MathF.Pow((float)InterestRate, i);
                }
                return MathF.Pow((float)initial, steps) + x * sum - (float)final;
            }

            // Derivative of f
            float Df(float x)
            {
                return (f(x + 0.0001f) - f(x)) / 0.0001f;
            }

            // Newton-Raphson method for finding roots of f
            while (f(delta) / Df(delta) >= 0.005f)
            {
                delta -= f(delta) / Df(delta);
            }


            for (var i = 1; i < steps; i++)
            {
                amount.Add(CalculateNextYearAmount(amount[i - 1], (decimal)delta));
                change.Add((decimal)delta);
            }

            change.Add((decimal)delta);

            return (amount, change);
        }

        protected decimal GetFutureValueAfter(int years, decimal withAdditions = 0.00M)
        {
            var futureValue = Value;
            for (var i = 0; i < years; i++)
            {
                futureValue += CalculateNextYearAmount(previousYearAmount: futureValue, yearDelta: withAdditions);
            }
            return futureValue;
        }

        protected decimal GetFutureValueAfter(int years, decimal startingFrom, decimal withAdditions = 0.00M)
        {
            var futureValue = startingFrom;
            for (var i = 0; i < years; i++)
            {
                futureValue += CalculateNextYearAmount(previousYearAmount: futureValue, yearDelta: withAdditions);
            }
            return futureValue;
        }

        protected virtual decimal CalculateNextYearAmount(decimal previousYearAmount, decimal yearDelta)
        {
            return previousYearAmount * InterestRate + yearDelta;
        }

        protected int GetRetirementLength(Data data)
        {
            return data.EndOfPlanDate - data.RetirementDate;
        }

        protected int GetTimeToRetirement(Data data)
        {
            return data.RetirementDate - DateTime.Today.Year;
        }

        protected abstract decimal CalculateTaxOn();
    }
}
