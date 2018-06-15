using System;
using System.Collections.Generic;
using tax_planning.Models.Tax_Calculation;

namespace tax_planning.Models
{
    public abstract class Asset
    {
        // Properties
        public string Name { get; set; }

        public string AssetType { get; set; }

        public decimal Value { get; set; }

        protected decimal _InterestRate = 0.06M;

        public decimal InterestRate
        {
            get
            {
                return _InterestRate * InterestRateMultiplier;
            }
            set
            {
                _InterestRate = value;
            }
        }

        public decimal Additions { get; set; }

        public bool Preferred { get; set; }

        public List<int> Years
        {
            get
            {
                List<int> list = new List<int>();

                for (var i = 0; i < YearlyAmount.Count; i++)
                {
                    list.Add(DateTime.Today.Year + i);
                }

                return list;
            }
        }

        public List<decimal> YearlyAmount { get; set; }

        public decimal Withdrawal { get; set; }

        public decimal AfterTaxWithdrawal { get; set; }

        public decimal TotalCashOut { get; set; }

        public decimal NetCashOut { get; set; }

        protected decimal InterestRateMultiplier { get; set; } = 0.0M;


        // Methods

        // Given desired additions, get withdrawal schedule
        // Not a constructor because assets may be initialized with empty properties
        public void CalculateSchedule()
        {
            // If InterestRateMultiplier is not overridden
            if (InterestRateMultiplier == 0.0M)
            {
                var liability = IncomeTaxCalculator.TotalIncomeTaxFor(Data.FilingStatus, Data.Income, Data.BasicAdjustment) / Data.Income;
                InterestRateMultiplier = 1 - liability;
            }

            var retirementLength = Data.EndOfPlanDate - Data.RetirementDate;
            var timeToRetirement = Data.RetirementDate - DateTime.Today.Year;

            List<decimal> amounts = new List<decimal>();

            // Add additions up to retirement
            for (var i = 0; i < timeToRetirement; i++)
            {
                amounts.Add(Decimal.Round(GetFutureValueAfter(years: i, withAdditions: (Additions - CalculateTaxOnAddition(Additions))), 2));
            }

            // Get the withdrawal
            var delta = GetWithdrawalFor(amounts[timeToRetirement - 1], retirementLength);

            // If InterestRateMultiplier is not overridden
            if (InterestRateMultiplier == 0.0M)
            {
                var liability = IncomeTaxCalculator.TotalIncomeTaxFor(Data.FilingStatus, Data.RetirementIncome, Data.BasicAdjustment) / Data.RetirementIncome;
                InterestRateMultiplier = 1 - liability;
            }

            // Populate the rest of the schedule
            for (var i = timeToRetirement; i < retirementLength + timeToRetirement; i++)
            {
                amounts.Add(Decimal.Round(CalculateNextYearAmount(amounts[i - 1], -delta), 2));
            }

            Withdrawal = Decimal.Round(delta, 2);
            AfterTaxWithdrawal = Decimal.Round(Withdrawal - CalculateTaxOnWithdrawal(Withdrawal), 2);
            YearlyAmount = amounts;
            TotalCashOut = Decimal.Round(AfterTaxWithdrawal * retirementLength, 2);
            NetCashOut = Decimal.Round(TotalCashOut - Additions, 2);
            Data.RetirementIncome += Withdrawal;
        }

        protected decimal GetWithdrawalFor(decimal principal, int steps)
        {
            // Payment calculation
            return (InterestRate * principal) / (1 - (decimal)Math.Pow(1 + (double)InterestRate, -steps));
        }

        protected decimal GetFutureValueAfter(int years, decimal withAdditions = 0.00M)
        {
            var futureValue = Value;
            for (var i = 0; i < years; i++)
            {
                futureValue = CalculateNextYearAmount(previousYearAmount: futureValue, yearDelta: withAdditions);
            }
            return futureValue;
        }

        protected virtual decimal CalculateNextYearAmount(decimal previousYearAmount, decimal yearDelta)
        {
            return previousYearAmount * (InterestRate + 1.00M) + yearDelta;
        }

        // Abstract methods
        protected abstract decimal CalculateTaxOnAddition(decimal addition);
        protected abstract decimal CalculateTaxOnWithdrawal(decimal withdrawal);
    }
}
