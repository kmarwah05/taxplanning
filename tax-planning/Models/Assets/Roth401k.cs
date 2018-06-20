﻿using System;
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
            get => Data.Additions[0];
            set
            {
                if (Match != null)
                {
                    Match.Additions = (value * EmployerMatchPercentage > Data.Income * EmployerMatchCap * EmployerMatchPercentage) ?
                            value * EmployerMatchCap * EmployerMatchPercentage :
                            value * EmployerMatchPercentage;
                }
            }
        }

        public override decimal Withdrawal
        {
            get
            {
                return _Withdrawal + (Match?.Withdrawal ?? 0);
            }
            set => _Withdrawal = value;
        }

        private decimal EmployerMatchPercentage { get; set; }
        private decimal EmployerMatchCap { get; set; }

        public Roth401k() : base() { }

        public Roth401k((decimal percentage, decimal cap) matching)
        {
            if (matching.percentage > 0.00M)
            {
                Match = new _401k()
                {
                    Name = "Match"
                };
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
