using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tax_planning.Models
{
    public interface IAsset
    {
        // Properties
        AssetType Type { get; }
        float Value { get; }
        DateTime StartDate { get;}

        // Methods
        float GetTaxRate();
    }
}
