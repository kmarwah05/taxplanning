using System;
using System.Collections.Generic;

namespace tax_planning.Models
{
    public class EquityHolding : Asset
    {
        protected override decimal CalculateTaxOn()
        {
            throw new NotImplementedException();
        }

        private decimal GetTaxForCapitalGain(decimal gain, FilingStatus filingStatus, decimal income)
        {
            int bracket = TaxBrackets.CapitalGainsBracketFor(filingStatus, income);
            return TaxBrackets.CapitalGainsRateForBracket[bracket] * gain;
        }
    }
}