using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tax_planning.Models
{
    public class AssetFactory
    {
        public static IAsset Create(AssetType assetType, float value, DateTime startDate) {
            return new AssetSketch(assetType, value, startDate);
        }
    }
}
