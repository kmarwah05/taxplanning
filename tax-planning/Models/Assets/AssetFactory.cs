using System;
using System.Collections.Generic;
using System.Linq;

namespace tax_planning.Models
{
    public class AssetFactory
    {
        public static List<Asset> Complete(List<Asset> partialAssets)
        {
            List<Asset> avoidSideEffects = new List<Asset>();
            List<string> assetTypes = new List<string>() { "IRA", "Roth IRA", "401k", "Roth 401k", "Brokerage Holding" };
            IEnumerable<string> typesToInstantiate = assetTypes.Where(type => !partialAssets.Select(asset => asset.AssetType).Contains(type));

            foreach (var type in typesToInstantiate)
            {
                avoidSideEffects.Add(Create(
                    name: type,
                    assetType: type,
                    value: 0.00M,
                    matching: (0.00M, 0.00M)
                ));
            }

            return avoidSideEffects;
        }

        // assetType string should be one of "IRA", "Roth IRA", "401k, Equity Holding"
        public static Asset Create(string name, string assetType, decimal value, (decimal percentage, decimal cap) matching) {
            switch (assetType)
            {
                case "IRA":
                    return new Ira()
                    {
                        Name = name,
                        AssetType = "IRA",
                        Value = value
                    };
                case "Roth IRA":
                    return new RothIra()
                    {
                        Name = name,
                        AssetType = "Roth IRA",
                        Value = value
                    };
                case "401k":
                    return new _401k(matching)
                    {
                        Name = name,
                        AssetType = "401k",
                        Value = value
                    };
                case "Roth 401k":
                    return new Roth401k(matching)
                    {
                        Name = name,
                        AssetType = "Roth 401k",
                        Value = value
                    };
                case "Brokerage Holding":
                    return new BrokerageHolding()
                    {
                        Name = name,
                        AssetType = "Brokerage Holding",
                        Value = value
                    };
                default:
                    throw new ArgumentException("Invalid asset type");
            }
        }
    }
}
