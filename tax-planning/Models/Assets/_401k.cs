﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tax_planning.Models
{
    public class _401k : TraditionalRetirementAsset
    {
        public static decimal MaxContributions => 18500.00M;
    }
}
