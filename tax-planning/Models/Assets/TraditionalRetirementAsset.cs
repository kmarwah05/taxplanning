using tax_planning.Models.Tax_Calculation;

namespace tax_planning.Models
{
    public abstract class TraditionalRetirementAsset : Asset
    {
        protected new decimal InterestRateMultiplier { get; set; } = 1.0M;

        protected override decimal CalculateTaxOnAddition(decimal addition)
        {
            var liability = IncomeTaxCalculator.TotalIncomeTaxFor(Data.FilingStatus, Data.Income, Data.BasicAdjustment) / Data.Income;
            return addition * liability;
        }

        protected override decimal CalculateTaxOnWithdrawal(decimal withdrawal) => 0.00M;
    }
}
