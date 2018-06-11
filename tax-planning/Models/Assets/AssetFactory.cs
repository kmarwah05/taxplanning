using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tax_planning.Models
{
    public class AssetFactory
    {
        // assetType string should be one of "IRA", "RothIRA", "_401k"
        public static IAsset Create(string name, string assetType, decimal value) {
            switch (assetType)
            {
                case "IRA":
                    return new Ira();
                case "RothIRA":
                    return new RothIra();
                case "_401k":
                    return new _401k();
                default:
                    throw new ArgumentException("Invalid asset type");
            }
        }
    }
}
