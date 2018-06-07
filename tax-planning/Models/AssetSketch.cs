using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tax_planning.Models
{
    public class AssetSketch : IAsset
    {
        public AssetType Type { get; private set; }

        public float Value { get; private set; }

        public DateTime StartDate { get; private set; }

        public AssetSketch(AssetType assetType, float value, DateTime startDate)
        {
            Type = assetType;
            Value = value;
            StartDate = startDate;
        }

        public float GetTaxRate()
        {
            throw new NotImplementedException();
        }
    }
}
