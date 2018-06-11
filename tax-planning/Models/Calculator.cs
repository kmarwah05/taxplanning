using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tax_planning.Models
{
    public class Calculator
    {
        public static Dictionary<string, Table> GetTablesFor(FormModel model)
        {
            return new Dictionary<string, Table>()
            {
                { "desired", GetDesiredScheduleFor(model) },
                { "optimal", GetOptimalScheduleFor(model) }
            };
        }

        public static Table GetOptimalScheduleFor(FormModel model)
        {
            Table table = new Table();
            Dictionary<DateTime, decimal> amountsForSchedule = new Dictionary<DateTime, decimal>();
            Dictionary<DateTime, decimal> taxesForSchedule = new Dictionary<DateTime, decimal>();

            var netWorth = model.Assets.Aggregate(0.00M, (sum, next) => sum += next.Value);
            var retirementLength = model.EndOfPlanDate.Year - model.RetirementDate.Year;
            var timeToRetirement = model.RetirementDate.Year - DateTime.Today.Year;
            var totalYearlyContribution = model.Assets.Aggregate(0.00M, (sum, next) => sum += next.YearlyGain);

            // Rough estimate to start, quality of estimation increases speed of Newton-Raphson
            var withdrawal = (totalYearlyContribution * (decimal)timeToRetirement + netWorth) / (decimal)retirementLength;
            // Initial Condition
            var amount = model.Assets.GetTotalValueForYear(model.RetirementDate.Year);

            // Goal-seeking with Newton-Raphson method
            Func<int, decimal> f = model.Assets.GetTotalValueForYear;
            decimal df(int year) => f(year + 1) - f(year);



            table.TotalCashOut = amountsForSchedule.Aggregate(0.00M, (sum, next) => next.Value < 0M ? sum -= next.Value : sum);
            table.TotalTaxPaid = taxesForSchedule.Aggregate(0.00M, (sum, next) => sum += next.Value);

            return table;
        }

        public static Table GetDesiredScheduleFor(FormModel model)
        {
            Dictionary<DateTime, decimal> amountsForSchedule = new Dictionary<DateTime, decimal>();
            Dictionary<DateTime, decimal> taxesForSchedule = new Dictionary<DateTime, decimal>();


            var totalCashOut = amountsForSchedule.Aggregate(0.00M, (sum, next) => next.Value > 0M ? sum += next.Value : sum);
            var totalTaxPaid = taxesForSchedule.Aggregate(0.00M, (sum, next) => sum += next.Value);

            // TODO: Implement calculator

            throw new NotImplementedException();
        }

        
    }
}
