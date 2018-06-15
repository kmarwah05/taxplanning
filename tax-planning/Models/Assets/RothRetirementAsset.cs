using tax_planning.Models.Tax_Calculation;

namespace tax_planning.Models
{
    public abstract class RothRetirementAsset : Asset
    {
        protected override decimal CalculateTaxOnAddition(decimal addition) => 0.00M;

        protected override decimal CalculateTaxOnWithdrawal(decimal withdrawal)
        {
            var liability = (IncomeTaxCalculator.FederalTaxFor(Data.FilingStatus, Data.Income, Data.BasicAdjustment) +
                IncomeTaxCalculator.VaStateTaxFor(Data.FilingStatus, Data.Income)) / Data.Income;
            return withdrawal * liability;
        }
    }
}
