using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tax_planning.Models
{
    public class Calculator
    {
        public static Response GetOptimalScheduleFor(FilingStatus filingStatus, decimal income, decimal basicAdjustment, decimal capitalGains, DateTime retirementDate, DateTime endOfPlan, List<IAsset> assets)
        {
            Dictionary<DateTime, decimal> amountsForOptimalSchedule = new Dictionary<DateTime, decimal>();
            Dictionary<DateTime, decimal> taxesForOptimalSchedule = new Dictionary<DateTime, decimal>();


            var totalCashOut = amountsForOptimalSchedule.Aggregate(0.00M, (sum, next) => next.Value > 0M ? sum += next.Value : sum);
            var totalTaxPaid = taxesForOptimalSchedule.Aggregate(0.00M, (sum, next) => sum += next.Value);

            throw new NotImplementedException();
        }
    }
}
