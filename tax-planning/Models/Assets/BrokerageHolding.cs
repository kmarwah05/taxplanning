using System;
using System.Collections.Generic;

namespace tax_planning.Models
{
    public class BrokerageHolding : Asset
    {

        protected override decimal CalculateTaxOnAddition(decimal addition) => 0.00M;

        protected override decimal CalculateTaxOnWithdrawal(decimal withdrawal) => 0.00M;

        private decimal GetTaxForCapitalGain(decimal gain, FilingStatus filingStatus, decimal income)
        {
            int bracket = TaxBrackets.CapitalGainsBracketFor(filingStatus, income);
            return TaxBrackets.CapitalGainsRateForBracket[bracket] * gain;
        }
    }
}