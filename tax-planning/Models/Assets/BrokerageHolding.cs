using tax_planning.Models.Tax_Calculation;

namespace tax_planning.Models
{
    public class BrokerageHolding : Asset
    {
        protected override decimal CalculateNextYearAmount(decimal previousYearAmount, decimal yearDelta, decimal currentIncome)
        {
            var withoutTax = base.CalculateNextYearAmount(previousYearAmount, yearDelta, currentIncome);
            return withoutTax - CalculateTaxOnDividend(withoutTax - previousYearAmount, currentIncome);
        }

        protected override decimal CalculateTaxOnAddition(decimal addition) => 0.00M;

        protected override decimal CalculateTaxOnWithdrawal(decimal withdrawal)
        {
            return IncomeTaxCalculator.TotalIncomeTaxFor(Data.FilingStatus, withdrawal, 0.00M); ;
        }

        protected decimal CalculateTaxOnDividend(decimal dividend, decimal currentIncome)
        {
            var bracket = TaxBrackets.CapitalGainsBracketFor(Data.FilingStatus, currentIncome);
            return TaxBrackets.CapitalGainsRateForBracket[bracket] * dividend;
        }
    }
}