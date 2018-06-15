using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tax_planning.Models
{
    public class Roth401k : RothRetirementAsset
    {
        public static decimal MaxContributions => 18500.00M;
    }
}
