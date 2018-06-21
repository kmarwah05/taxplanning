using System;
using System.Linq;

namespace tax_planning.Models
{
    public class Roth401k : RothRetirementAsset
    {
        private decimal _Withdrawal;

        public static decimal MaxContributions = 18500.00M;

        private _401k Match { get; set; }

        public override decimal Additions
        {
            get
            {
                if (Name.Equals("Match"))
                {
                    return (Data.Additions[0] * Data.EmployerMatchPercentage > Data.Income * Data.EmployerMatchCap * Data.EmployerMatchPercentage) ?
                            Data.Income * Data.EmployerMatchCap * Data.EmployerMatchPercentage :
                            Data.Additions[0] * Data.EmployerMatchPercentage;
                }
                return Data.Additions[0];
            }
            set => base.Additions = value;
        }

        public override decimal Withdrawal
        {
            get
            {
                return _Withdrawal + (Match?.Withdrawal ?? 0);
            }
            set => _Withdrawal = value;
        }

        public decimal WithdrawalFromEmployerContribution => Match?.Withdrawal ?? 0;

        public Roth401k() {
            if (Data.EmployerMatchPercentage > 0.00M)
            {
                Match = new _401k("Match");
            }
        }

        public override void CalculateSchedule()
        {
            base.CalculateSchedule();
            Match?.CalculateSchedule();
        }

        public override void CalculateTaxInfo()
        {
            if (Match != null)
            {
                base.CalculateSchedule();
                Match.CalculateTaxInfo();
                YearlyAmount = YearlyAmount.Select((amount, index) => amount + Match.YearlyAmount[index]).ToList();
                AfterTaxWithdrawal = Decimal.Round(_Withdrawal - CalculateTaxOnWithdrawal(_Withdrawal, Data.RetirementIncome), 2) + Match.AfterTaxWithdrawal;
                TotalCashOut = Decimal.Round(AfterTaxWithdrawal * RetirementLength, 2) + Match.TotalCashOut;
                NetCashOut = Decimal.Round(TotalCashOut - (Additions * TimeToRetirement), 2) + Match.TotalCashOut;
            }
            else
            {
                base.CalculateTaxInfo();
            }
        }

        public override void UpdateCapsFor(int age)
        {
            if (age >= 50 && !Name.Equals("Match"))
            {
                MaxContributions = 24500.00M;
            }
            else
            {
                MaxContributions = 18500.00M;
            }
        }
    }
}
