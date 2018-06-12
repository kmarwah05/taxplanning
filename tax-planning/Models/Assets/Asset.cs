using System;
using System.Collections.Generic;
using System.Linq;

namespace tax_planning.Models
{
    public abstract class Asset
    {
        // Properties
        public string Name { get; set; }

        public decimal Value { get; set; }

        public decimal InterestRate { get; set; }


        // Methods
        public virtual Table GetOptimalScheduleFor(FormModel model)
        {
            Table table = new Table();

            var netWorth = model.Assets.Aggregate(0.00M, (sum, next) => sum + next.Value);
            var retirementLength = model.EndOfPlanDate - model.RetirementDate;
            var timeToRetirement = model.RetirementDate - DateTime.Today.Year;
            var totalYearlyContribution = model.Assets.Aggregate(0.00M, (sum, next) => sum + next.InterestRate); //TODO: update formula

            // Rough estimate to start, quality of estimation increases speed of Newton-Raphson
            var withdrawal = (totalYearlyContribution * (decimal)timeToRetirement + netWorth) / (decimal)retirementLength;
            // Initial Condition
            var amount = model.Assets.GetTotalValueForYear(model.RetirementDate);

            // Do calculation
            (List<decimal> amounts, List<decimal> taxes) schedule = GetScheduleWith((float)withdrawal, amount, 0.00M, retirementLength);

            List<decimal> amountsForSchedule = new List<decimal>();
            List<decimal> taxesForSchedule = new List<decimal>();

            return table;
        }

        public virtual Table GetDesiredScheduleFor(FormModel model)
        {
            Table table = new Table();

            var netWorth = model.Assets.Aggregate(0.00M, (sum, next) => sum += next.Value);
            var retirementLength = model.EndOfPlanDate - model.RetirementDate;
            var timeToRetirement = model.RetirementDate - DateTime.Today.Year;
            var totalYearlyContribution = model.Assets.Aggregate(0.00M, (sum, next) => sum += next.InterestRate);

            // Rough estimate to start, quality of estimation increases speed of Newton-Raphson
            var withdrawal = (totalYearlyContribution * (decimal)timeToRetirement + netWorth) / (decimal)retirementLength;
            // End condition
            // TODO: Calculate peak amount needed

            // Do calculation
            (List<decimal> amounts, List<decimal> taxes) schedule = GetScheduleWith((float)totalYearlyContribution, netWorth, 0.00M // TODO: fix this
                , retirementLength);

            List<decimal> amountsForSchedule = new List<decimal>();
            List<decimal> taxesForSchedule = new List<decimal>();


            // Populate dicts

            return table;
        }

        public virtual decimal CalculateNextYearAmount(decimal previousYearAmount, decimal yearDelta)
        {
            return previousYearAmount * InterestRate + yearDelta;
        }

        protected virtual (List<decimal> amounts, List<decimal> taxes) GetScheduleWith(float delta, decimal initial, decimal final, int steps)
        {
            List<decimal> amounts = new List<decimal>() { initial };
            List<decimal> taxes = new List<decimal>() { CalculateTaxOn(initial) };

            // Amount at final
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


            for (var i = 1; i < amounts.Count; i++)
            {
                amounts[i] = CalculateNextYearAmount(amounts[i - 1], (decimal)delta);
                taxes[i] = CalculateTaxOn(amounts[i]);
            }

            return (amounts, taxes);
        }

        protected virtual (List<decimal> amounts, List<decimal> taxes) GetPreWithdrawalSchedule(decimal additions, int numberOfYears, float interestRate)
        {
            throw new NotImplementedException();
        }
        //TODO: implement
        protected virtual decimal CalculateTaxOn(decimal amount)
        {
            return -1;
        }
    }
}
