using System;
using System.Linq;

namespace tax_planning.Models.Tax_Calculation
{
    public class IncomeTaxCalculator
    {
        public static decimal FederalTaxFor(FilingStatus status, decimal income, decimal basicAdjustment) => CalculateGraduatedTaxFor(status, "Federal", income, basicAdjustment);

        public static decimal VaStateTaxFor(FilingStatus status, decimal income) => CalculateGraduatedTaxFor(status, "VA State", income, 0.00M);

        private static decimal CalculateGraduatedTaxFor(FilingStatus status, string jurisdiction, decimal income, decimal basicAdjustment)
        {
            // Get jurisdiction data
            (decimal lowerBound, decimal upperBound)[] brackets;
            float[] rateForBracket;

            switch (jurisdiction) {
                case "Federal":
                    brackets = TaxBrackets.FederalIncomeBracketsFor(filingStatus: status);
                    rateForBracket = TaxBrackets.FederalIncomeRateForBracket.Select(x => (float)x).ToArray();
                    break;
                case "VA State":
                    brackets = TaxBrackets.FederalIncomeBracketsFor(filingStatus: status);
                    rateForBracket = TaxBrackets.FederalIncomeRateForBracket.Select(x => (float)x).ToArray();
                    break;
                default:
                    Console.WriteLine("Jurisdiction not supported");
                    return 0.00M;
            }
            
            // Standard deduction
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


            // After standard deduction
            var ranges = brackets.Select(bracket => bracket.upperBound - bracket.lowerBound);

            var tax = 0.00f;

            var i = 0;
            while (income > brackets[i].upperBound)
            {
                tax += (float)ranges.ElementAt(i) * rateForBracket[i];
                i++;
            }

            var cherryOnTop = (float)(income - brackets[i].lowerBound - basicAdjustment - ranges.Take(i).Sum()) * rateForBracket[i];

            return (decimal)(tax + cherryOnTop);
        }
    }
}
