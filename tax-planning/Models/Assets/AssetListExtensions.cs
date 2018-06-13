using System;
using System.Collections.Generic;
using System.Linq;
using tax_planning.Models;

namespace tax_planning.Extensions
{
    public static class AssetListExtensions {
        public static Table GetOptimalScheduleFor(this List<Asset> assets, Data data)
        {
            return assets.Aggregate(assets.First().GetOptimalScheduleFor(data), (a, b) => a + b.GetOptimalScheduleFor(data));
        }

        public static Table GetDesiredScheduleFor(this List<Asset> assets, Data data)
        {
            return assets.Aggregate(assets.First().GetDesiredScheduleFor(data), (a, b) => a + b.GetDesiredScheduleFor(data));
        }
    }
}
