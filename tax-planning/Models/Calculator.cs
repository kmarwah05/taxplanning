using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tax_planning.Models
{
    public class Calculator
    {
        public static Dictionary<string, Table> GetTablesFor(
            FilingStatus filingStatus,
            decimal income,
            decimal basicAdjustment,
            decimal capitalGains,
            DateTime retirementDate,
            DateTime endOfPlanDate,
            List<IAsset> assets)
        {
            return new Dictionary<string, Table>()
            {
                { "desired", GetDesiredScheduleFor(filingStatus, income, basicAdjustment, capitalGains, retirementDate, endOfPlanDate, assets) },
                { "optimal", GetOptimalScheduleFor(filingStatus, income, basicAdjustment, capitalGains, retirementDate, endOfPlanDate, assets) }
            };
        }

        public static Table GetOptimalScheduleFor(
            FilingStatus filingStatus,
            decimal income,
            decimal basicAdjustment,
            decimal capitalGains,
            DateTime retirementDate,
            DateTime endOfPlanDate,
            List<IAsset> assets)
        {
            Dictionary<DateTime, decimal> amountsForSchedule = new Dictionary<DateTime, decimal>();
            Dictionary<DateTime, decimal> taxesForSchedule = new Dictionary<DateTime, decimal>();


            var totalCashOut = amountsForSchedule.Aggregate(0.00M, (sum, next) => next.Value < 0M ? sum -= next.Value : sum);
            var totalTaxPaid = taxesForSchedule.Aggregate(0.00M, (sum, next) => sum += next.Value);

            // TODO: Implement calculator

            throw new NotImplementedException();
        }

        public static Table GetDesiredScheduleFor(
            FilingStatus filingStatus,
            decimal income,
            decimal basicAdjustment,
            decimal capitalGains,
            DateTime retirementDate,
            DateTime endOfPlanDate,
            List<IAsset> assets)
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
