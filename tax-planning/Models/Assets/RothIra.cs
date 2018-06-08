using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tax_planning.Models
{
    public class RothIra : IAsset
    {
        public float Value => throw new NotImplementedException();

        public decimal YearlyAdditions => throw new NotImplementedException();

        public DateTime WithdrawalStartDate => throw new NotImplementedException();

        public List<decimal> CalculateOptimalWithdrawals()
        {
            throw new NotImplementedException();
        }
    }
}
