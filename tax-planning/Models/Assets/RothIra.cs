using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tax_planning.Models
{
    public class RothIra : RothRetirementAsset
    {
        public static decimal MaxContributions => 5500.00M;
    }
}
