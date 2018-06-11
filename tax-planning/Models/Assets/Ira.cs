using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tax_planning.Models
{
    public class Ira : IAsset
    {
        public string Name => throw new NotImplementedException();

        public decimal Value => throw new NotImplementedException();

        public decimal YearlyAdditions => throw new NotImplementedException();

        public DateTime WithdrawalStartDate => throw new NotImplementedException();

        public List<decimal> CalculateOptimalWithdrawals()
        {
            throw new NotImplementedException();
        }

        public decimal CalculatePeakAmountWith(decimal additions, int numberOfYears, float interestRate)
        {
            return -1;
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
