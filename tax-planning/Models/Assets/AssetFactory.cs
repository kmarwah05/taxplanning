using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tax_planning.Models
{
    public class AssetFactory
    {
        public static IAsset Create(string name, AssetType assetType, decimal value) {
            switch (assetType)
            {
                case AssetType.Ira:
                    return new Ira();
                case AssetType.RothIra:
                    return new RothIra();
                case AssetType._401k:
                    return new _401k();
                default:
                    throw new ArgumentException("Invalid asset type");
            }
        }
    }
}
