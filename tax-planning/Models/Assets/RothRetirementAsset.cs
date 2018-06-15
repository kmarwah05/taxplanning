using tax_planning.Models.Tax_Calculation;

namespace tax_planning.Models
{
    public abstract class RothRetirementAsset : Asset
    {
        protected new decimal InterestRateMultiplier { get; set; } = 1.0M;

        protected override decimal CalculateTaxOnAddition(decimal addition) => 0.00M;

        protected override decimal CalculateTaxOnWithdrawal(decimal withdrawal)
        {
            var liability = IncomeTaxCalculator.TotalIncomeTaxFor(Data.FilingStatus, Data.Income, Data.BasicAdjustment) / Data.Income;
            return withdrawal * liability;
        }
    }
}
