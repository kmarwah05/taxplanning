using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tax_planning.Models
{
    public class AssetSketch : IAsset
    {

        public float Value { get; private set; }

        public DateTime StartDate { get; private set; }

        public AssetSketch(float value, DateTime startDate)
        {
            Value = value;
            StartDate = startDate;
        }

        public float GetTaxRate()
        {
            throw new NotImplementedException();
        }

        public float GetInterestRate()
        {
            throw new NotImplementedException();        }
    }
}
