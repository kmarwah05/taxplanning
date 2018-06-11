using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tax_planning.Models
{
    public class AssetFactory
    {
        // assetType string should be one of "Ira", "RothIra", "_401k"
        public static IAsset Create(string name, string assetType, decimal value) {
            switch (assetType)
            {
                case "Ira":
                    return new Ira();
                case "RothIra":
                    return new RothIra();
                case "_401k":
                    return new _401k();
                case "EquityHolding":
                    return new EquityHolding();
                default:
                    throw new ArgumentException("Invalid asset type");
            }
        }
    }
}
