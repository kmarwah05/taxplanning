using System;
using System.Collections.Generic;

namespace tax_planning.Models
{
    public class EquityHolding : IAsset
    {
        public string Name { get; set; }

        public decimal Value { get; set; }

        public decimal InterestRate { get; set; }

        public decimal CalculateNextYearAmount(decimal previousYearAmount, decimal yearDelta)
        {
            return previousYearAmount + InterestRate + yearDelta;
        }

        private decimal GetTaxForCapitalGain(decimal gain, FilingStatus filingStatus, decimal income)
        {
            int bracket = TaxBrackets.CapitalGainsBracketFor(filingStatus, income);
            return TaxBrackets.CapitalGainsRateForBracket[bracket] * gain;
        }
    }
}