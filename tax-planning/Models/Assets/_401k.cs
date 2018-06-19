using System;
using System.Linq;

namespace tax_planning.Models
{
    public class _401k : TraditionalRetirementAsset
    {
        private decimal _Withdrawal;

        public static decimal MaxContributions = 18500.00M;

        public override decimal Additions
        {
            get => Data.Additions[0];
            set
            {
                if (Match != null)
                {
                    Match.Additions = (value * EmployerMatchPercentage > Data.Income * EmployerMatchCap) ?
                            Data.Income * EmployerMatchCap :
                            value * EmployerMatchPercentage;
                }
            }
        }

        public override decimal Withdrawal {
            get
            {
                return _Withdrawal + (Match?.Withdrawal ?? 0);
            }
            set => _Withdrawal = value;
        }

        private _401k Match { get; set; }

        private decimal EmployerMatchPercentage { get; set; }
        private decimal EmployerMatchCap { get; set; }

        public _401k() : base() { }

        public _401k((decimal percentage, decimal cap) matching)
        {
            if (matching.percentage > 0.00M)
            {
                Match = new _401k();
                EmployerMatchPercentage = matching.percentage;
                EmployerMatchCap = matching.cap;
            }
        }

        public override void CalculateSchedule()
        {
            base.CalculateSchedule();
            Match?.CalculateSchedule();
        }

        public override void CalculateData()
        {
            if (Match != null)
            {
                Match.CalculateData();
                YearlyAmount = YearlyAmount.Select((amount, index) => amount + Match.YearlyAmount[index]).ToList();
                AfterTaxWithdrawal = Decimal.Round(_Withdrawal - CalculateTaxOnWithdrawal(_Withdrawal, Data.RetirementIncome), 2) + Match.AfterTaxWithdrawal;
                TotalCashOut = Decimal.Round(AfterTaxWithdrawal * RetirementLength, 2) + Match.TotalCashOut;
                NetCashOut = Decimal.Round(TotalCashOut - (Additions * TimeToRetirement), 2) + Match.TotalCashOut;
            }
            else
            {
                base.CalculateData();
            }
        }

        public override void UpdateCapsFor(int age)
        {
            if (age >= 50)
            {
                MaxContributions = 22500.00M;
            }
        }
    }
}
