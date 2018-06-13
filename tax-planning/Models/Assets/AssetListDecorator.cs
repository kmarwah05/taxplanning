using System;
using System.Collections.Generic;
using System.Linq;

namespace tax_planning.Models.Assets
{
    public class AssetListDecorator : List<Asset>
    {
        public AssetListDecorator() : base() { }

        public decimal GetTotalValueForYear(int year)
        {
            return this.Aggregate(0.00M, (sum, next) =>
            {
                var total = next.Value;
                for (var i = DateTime.Today.Year; i < year; i++)
                {
                    total += next.CalculateNextYearAmount(previousYearAmount: total, yearDelta: next.InterestRate * next.Value);
                }
                return sum += total;
            });
        }

        public Table GetOptimalScheduleFor(FormModel model)
        {
            return this.Aggregate(this.First().GetOptimalScheduleFor(model), (a, b) => a + b.GetOptimalScheduleFor(model));
        }

        public Table GetDesiredScheduleFor(FormModel model)
        {
            return this.Aggregate(this.First().GetDesiredScheduleFor(model), (a, b) => a + b.GetDesiredScheduleFor(model));
        }
    }
}
