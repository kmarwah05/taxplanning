using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tax_planning.Models
{
    public class Ira : TraditionalRetirementAsset
    {
        public static decimal MaxContributions => 5500.00M;
    }
}
