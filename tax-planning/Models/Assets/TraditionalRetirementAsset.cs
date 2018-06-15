using tax_planning.Models.Tax_Calculation;

namespace tax_planning.Models
{
    public abstract class TraditionalRetirementAsset : Asset
    {
        protected override decimal CalculateTaxOnAddition(decimal addition)
        {
            var liability = (IncomeTaxCalculator.FederalTaxFor(Data.FilingStatus, Data.Income, Data.BasicAdjustment) +
                IncomeTaxCalculator.VaStateTaxFor(Data.FilingStatus, Data.Income)) / Data.Income;
            return addition * liability;
        }

        protected override decimal CalculateTaxOnWithdrawal(decimal withdrawal) => 0.00M;
    }
}
