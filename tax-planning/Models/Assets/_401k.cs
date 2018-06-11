using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tax_planning.Models
{
    public class _401k : IAsset
    {
        public string Name { get; set; }

        public decimal Value { get; set; }

        public decimal YearlyAdditions { get; set; }

        public DateTime WithdrawalStartDate { get; set; }

        public decimal YearlyGain { get; set; }

        public List<decimal> CalculateOptimalWithdrawals()
        {
            throw new NotImplementedException();
        }

        public decimal CalculatePeakAmountWith(decimal additions, int numberOfYears, float interestRate)
        {
            return -1;
        }

        public decimal CalculateNextYearAmount(decimal previousYearAmount, decimal yearDelta)
        {
            return previousYearAmount + YearlyGain + yearDelta;
        }

        private decimal GetTaxForAddition(decimal amount, FilingStatus filingStatus)
        {
            return -1;
        }

        private decimal GetTaxForWithdrawal(decimal amount)
        {
            throw new NotImplementedException();
        }
    }
}
