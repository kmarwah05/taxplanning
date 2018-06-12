using System;
using System.Collections.Generic;

namespace tax_planning.Models
{
    public class EquityHolding : Asset
    {

        private decimal GetTaxForCapitalGain(decimal gain, FilingStatus filingStatus, decimal income)
        {
            int bracket = TaxBrackets.CapitalGainsBracketFor(filingStatus, income);
            return TaxBrackets.CapitalGainsRateForBracket[bracket] * gain;
        }
    }
}