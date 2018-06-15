using tax_planning.Models.Tax_Calculation;

namespace tax_planning.Models
{
    public abstract class TraditionalRetirementAsset : Asset
    {
        protected override decimal CalculateTaxOnAddition(decimal addition) => 0.00M;

        protected override decimal CalculateTaxOnWithdrawal(decimal withdrawal)
        {
            var liability = IncomeTaxCalculator.TotalIncomeTaxFor(Data.FilingStatus, Data.Income, Data.BasicAdjustment) / Data.Income;
            return withdrawal * liability;
        }

        public TraditionalRetirementAsset()
        {
            InterestRateMultiplier = 1.0M;
        }
    }
}
