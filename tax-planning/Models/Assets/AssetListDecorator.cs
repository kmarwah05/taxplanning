using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tax_planning.Models.Assets
{
    public class AssetListDecorator : List<IAsset>
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

        public decimal GetAmountAfter(int duration, decimal withdrawal)
        {

        }
    }
}
