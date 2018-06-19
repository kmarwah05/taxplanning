using tax_planning.Models.Tax_Calculation;

namespace tax_planning.Models
{
    public class BrokerageHolding : Asset
    {
        public override decimal Additions { get => Data.Additions[2]; set => base.Additions = value; }

        protected override decimal CalculateTaxOnAddition(decimal addition) => 0.00M;

        protected override decimal CalculateTaxOnWithdrawal(decimal withdrawal, decimal income)
        {
            return IncomeTaxCalculator.CapitalGainsTaxFor(Data.FilingStatus, withdrawal, income);
        }

        public override void UpdateCapsFor(int age)
        {
            return;
        }
    }
}