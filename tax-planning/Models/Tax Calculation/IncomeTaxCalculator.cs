using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tax_planning.Models.Tax_Calculation
{
    public class IncomeTaxCalculator
    {
        public static decimal CalculateTaxForIncome(FilingStatus status, decimal income, decimal basicAdjustment)
        {
            var standardDeduction = 0.00M;
            switch (status)
            {
                case FilingStatus.Joint:
                    standardDeduction = 24000.00M;
                    break;
                case FilingStatus.HeadOfHousehold:
                    standardDeduction = 18000.00M;
                    break;
                case FilingStatus.MarriedSeparate:
                case FilingStatus.Unmarried:
                default:
                    standardDeduction = 12000;
                    break;
            }

            income -= standardDeduction;

            var brackets = TaxBrackets.IncomeBracketsFor(status);
            var ranges = brackets.Select(bracket => bracket.upperBound - bracket.lowerBound);

            var tax = 0.00M;

            var i = 0;
            while (income > brackets[i].upperBound)
            {
                tax += ranges.ElementAt(i) * TaxBrackets.IncomeRateForBracket[i];
                i++;
            }

            var cherryOnTop = (income - brackets[i].lowerBound - basicAdjustment - ranges.Take(i).Sum()) * TaxBrackets.IncomeRateForBracket[i];

            return tax + cherryOnTop;
        }
    }
}
