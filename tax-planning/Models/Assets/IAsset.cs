using System;
using System.Collections.Generic;

namespace tax_planning.Models
{
    public interface IAsset
    {
        // Properties
        string Name { get; }
        decimal Value { get; }
    }
}
