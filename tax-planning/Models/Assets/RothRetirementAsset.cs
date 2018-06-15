using tax_planning.Models.Tax_Calculation;

namespace tax_planning.Models
{
    public abstract class RothRetirementAsset : Asset
    {
        protected override decimal CalculateTaxOnAddition(decimal addition)
        {
            var liability = IncomeTaxCalculator.TotalIncomeTaxFor(Data.FilingStatus, Data.Income, Data.BasicAdjustment) / Data.Income;
            return addition * liability;
        }

        protected override decimal CalculateTaxOnWithdrawal(decimal withdrawal) => 0.00M;

        public RothRetirementAsset()
        {
            InterestRateMultiplier = 1.0M;
        }
    }
}
