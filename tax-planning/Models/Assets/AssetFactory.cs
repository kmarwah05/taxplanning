using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tax_planning.Models
{
    public class AssetFactory
    {
        // assetType string should be one of "IRA", "Roth IRA", "401k, Equity Holding"
        public static IAsset Create(string name, string assetType, decimal value, decimal yearlyAdditions = 0.00M) {
            switch (assetType)
            {
                case "IRA":
                    return new Ira()
                    {
                        Name = name,
                        Value = value,
                        YearlyAdditions = yearlyAdditions
                    };
                case "Roth IRA":
                    return new RothIra()
                    {
                        Name = name,
                        Value = value,
                        YearlyAdditions = yearlyAdditions
                    };
                case "401k":
                    return new _401k()
                    {
                        Name = name,
                        Value = value,
                        YearlyAdditions = yearlyAdditions
                    };
                case "Equity Holding":
                    return new EquityHolding()
                    {
                        Name = name,
                        Value = value
                    };
                default:
                    throw new ArgumentException("Invalid asset type");
            }
        }
    }
}
