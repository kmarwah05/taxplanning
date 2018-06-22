using tax_planning.Models.Tax_Calculation;

namespace tax_planning.Models
{
    public class RothIra : RothRetirementAsset
    {
        public override decimal Additions
        {
            get
            {
                // IRS caps on Roth IRA additions
                var agi = IncomeTaxCalculator.GetAdjustedGrossIncome(Data.FilingStatus, Data.Income, "Federal");
                if (Data.FilingStatus == FilingStatus.Joint)
                {
                    if (agi >= 189000)
                    {
                        return 0;
                    }
                }
                else
                {
                    if (agi >= 120000)
                    {
                        return 0;
                    }
                }

                return Data.Additions[1];
            }
        }

        public static decimal MaxContributions = 5500.00M;

        public override void UpdateCapsFor(int age)
        {
            if (age >= 50)
            {
                MaxContributions = 6500.00M;
            }
            else
            {
                MaxContributions = 5500.00M;
            }
        }
    }
}
