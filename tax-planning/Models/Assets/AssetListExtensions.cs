using System;
using System.Collections.Generic;
using System.Linq;
using tax_planning.Models;

namespace tax_planning.Extensions
{
    public static class AssetListExtensions {
        public static Table GetOptimalScheduleFor(this List<Asset> assets, Data data)
        {
            var length = assets.First().GetOptimalScheduleFor(data).Years.Count;
            var table = new Table()
            {
                YearlyAmount = new List<decimal>(),
                YearlyChange = new List<decimal>()
            };
            table.YearlyAmount.AddRange(new decimal[length]);
            table.YearlyChange.AddRange(new decimal[length]);

            return assets.Aggregate(table, (a, b) => a + b.GetOptimalScheduleFor(data));
        }

        public static Table GetDesiredScheduleFor(this List<Asset> assets, Data data)
        {
            var length = assets.First().GetOptimalScheduleFor(data).Years.Count;
            var table = new Table()
            {
                YearlyAmount = new List<decimal>(),
                YearlyChange = new List<decimal>()
            };
            table.YearlyAmount.AddRange(new decimal[length]);
            table.YearlyChange.AddRange(new decimal[length]);

            return assets.Aggregate(table, (a, b) => a + b.GetDesiredScheduleFor(data));
        }
    }
}
