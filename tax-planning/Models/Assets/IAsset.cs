using System;
using System.Collections.Generic;

namespace tax_planning.Models
{
    public interface IAsset
    {
        // Properties
        string Name { get; set; }
        decimal Value { get; set; }
        decimal InterestRate { get; set; }

        decimal CalculateNextYearAmount(decimal previousYearAmount, decimal yearDelta);
    }
}
