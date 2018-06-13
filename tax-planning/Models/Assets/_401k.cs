using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tax_planning.Models
{
    public class _401k : Asset
    {
        public decimal YearlyAdditions { get; set; }

        public DateTime WithdrawalStartDate { get; set; }

        protected override decimal CalculateTaxOn()
        {
            throw new NotImplementedException();
        }

        private decimal CalculatePeakAmountWith(decimal additions, int numberOfYears, float interestRate)
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
