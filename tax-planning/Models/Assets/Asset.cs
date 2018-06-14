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
            var withdrawalGuess = -(additions * timeToRetirement + Value) / retirementLength;

            // Initial Condition
            var amount = GetFutureValueAfter(years: timeToRetirement, withAdditions: additions);

            // Do calculation
            (List<decimal> amount, decimal change) retirementSchedule = GetScheduleWith((double)withdrawalGuess, (double)amount, 0.0f, retirementLength);

            // Populate entire schedules
            List<decimal> amountForSchedule = new List<decimal>();
            List<decimal> changeForSchedule = new List<decimal>();

            for (var i = 0; i < timeToRetirement; i++)
            {
                amountForSchedule.Add(GetFutureValueAfter(years: i, withAdditions: data.DesiredAdditions));
                changeForSchedule.Add(data.DesiredAdditions);
            }

            amountForSchedule.AddRange(retirementSchedule.amount);
            for (var i = 0; i < retirementLength; i++)
            {
                changeForSchedule.Add(retirementSchedule.change);
            }

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
            var peak = 0.00M;

            for (var i = 0; i < retirementLength; i++)
            {
                peak = (peak + withdrawal) / (InterestRate + 1.00M);
            }

            peak = -peak;

            // Guess
            decimal additions = (peak - Value) / timeToRetirement;

            // Do calculation, gives partial schedule
            (List<decimal> amount, decimal change) preRetirementSchedule = GetScheduleWith((double)additions, (double)Value, (double)peak, retirementLength);

            // Populate entire schedules
            List<decimal> amountForSchedule = new List<decimal>();
            List<decimal> changeForSchedule = new List<decimal>();

            amountForSchedule.AddRange(preRetirementSchedule.amount);

            for (var i = 0; i < timeToRetirement; i++) {
                changeForSchedule.Add(preRetirementSchedule.change);
            }

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

        protected virtual (List<decimal> amount, decimal change) GetScheduleWith(double guess, double initial, double final, int steps)
        {
            var delta = GetDeltaFor(guess, initial, final, steps);

            List<decimal> amount = new List<decimal>() { (decimal)initial };

            for (var i = 1; i < steps; i++)
            {
                amount.Add(CalculateNextYearAmount(amount[i - 1], delta));
            }

            return (amount, delta);
        }

        protected decimal GetDeltaFor(double guess, double initial, double final, int steps)
        {
            // Amount at final, want f(x) = 0
            // Summation k=0 to k=steps-1 of I^k
            var sum = 0.0;
            for (var i = 0; i < steps - 1; i++)
            {
                sum += Math.Pow(((double)InterestRate + 1.0), i);
            }

            var principalMultiplier = Math.Pow(((double)InterestRate + 1.0f), steps);

            double f(double x)
            {
                // Formula for final amount
                return initial * principalMultiplier + x * sum - final;
            }

            // Derivative of f (approximately)
            double Df(double x)
            {
                return (f(x + 0.00001) - f(x)) / 0.00001;
            }

            // DF != Infinity

            double ans = ApproximateRoot(f, Df, guess);
            return (decimal)ans;
        }

        // Newton-Raphson method for root finding
        protected double ApproximateRoot(Func<double, double> f, Func<double, double> Df, double guess)
        {
            var delta = guess;
            var currentError = f(delta) / Df(delta);
            while (Math.Abs(currentError) >= 0.0049 && Df(delta) != 0)
            {
                delta -= currentError;
                currentError = f(delta) / Df(delta);
            }

            return delta;
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

        protected decimal GetFutureValueAfter(int years, decimal startingFrom, decimal withAdditions = 0.00M)
        {
            var futureValue = startingFrom;
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
