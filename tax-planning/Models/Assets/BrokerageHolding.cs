using tax_planning.Models.Tax_Calculation;

namespace tax_planning.Models
{
    public class BrokerageHolding : Asset
    {
        protected override decimal CalculateTaxOnAddition(decimal addition) => 0.00M;

        protected override decimal CalculateTaxOnWithdrawal(decimal withdrawal)
        {
            return IncomeTaxCalculator.TotalIncomeTaxFor(Data.FilingStatus, withdrawal, 0.00M); ;
        }
    }
}