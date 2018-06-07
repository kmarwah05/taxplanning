using System;

namespace tax_planning.Models
{
    public interface IAsset
    {
        // Properties
        float Value { get; }
        DateTime StartDate { get;}

        // Methods
        float GetTaxRate();
    }
}
