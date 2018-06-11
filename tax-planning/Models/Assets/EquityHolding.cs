using System;
using System.Collections.Generic;

namespace tax_planning.Models
{
    internal class EquityHolding : IAsset
    {
        public string Name => throw new NotImplementedException();

        public decimal Value => throw new NotImplementedException();

        private decimal GetTaxForCapitalGain(decimal gain, FilingStatus filingStatus, decimal income)
        {
            int bracket = TaxBrackets.CapitalGainsBracketFor(filingStatus, income);
            return TaxBrackets.CapitalGainsRateForBracket[bracket] * gain;
        }
    }
}