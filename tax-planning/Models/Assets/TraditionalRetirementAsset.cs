using tax_planning.Models.Tax_Calculation;

namespace tax_planning.Models
{
    public abstract class TraditionalRetirementAsset : Asset
    {
        protected override decimal CalculateTaxOnAddition(decimal addition) => 0.00M;

        protected override decimal CalculateTaxOnWithdrawal(decimal withdrawal, decimal income)
        {
            var liability = IncomeTaxCalculator.TotalIncomeTaxFor(Data.FilingStatus, income, 0.00M) / income;
            return withdrawal * liability;
        }
    }
}
