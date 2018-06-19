using System;
using System.Linq;

namespace tax_planning.Models.Tax_Calculation
{
    public class IncomeTaxCalculator
    {
        public static decimal CapitalGainsTaxFor(FilingStatus status, decimal gain, decimal income)
        {
            income -= GetStandardDeduction(filingStatus: status, jurisdiction: "Federal");
            var bracket = TaxBrackets.CapitalGainsBracketFor(status, income);
            var rate = TaxBrackets.CapitalGainsRateForBracket[bracket];
            return gain * rate;
        }

        public static decimal TotalIncomeTaxFor(FilingStatus status, decimal income, decimal basicAdjustment) =>
            FederalTaxFor(status, income, basicAdjustment) + VaStateTaxFor(status, income);

        public static decimal FederalTaxFor(FilingStatus status, decimal income, decimal basicAdjustment) => CalculateGraduatedTaxFor(status, "Federal", income, basicAdjustment);

        public static decimal VaStateTaxFor(FilingStatus status, decimal income) => CalculateGraduatedTaxFor(status, "VA State", income, 0.00M);

        private static decimal CalculateGraduatedTaxFor(FilingStatus status, string jurisdiction, decimal income, decimal basicAdjustment)
        {
            // Get jurisdiction data
            (decimal lowerBound, decimal upperBound)[] brackets;
            float[] rateForBracket;

            switch (jurisdiction)
            {
                case "Federal":
                    brackets = TaxBrackets.FederalIncomeBracketsFor(filingStatus: status);
                    rateForBracket = TaxBrackets.FederalIncomeRateForBracket.Select(x => (float)x).ToArray();
                    break;
                case "VA State":
                    brackets = TaxBrackets.VaStateBrackets();
                    rateForBracket = TaxBrackets.VaStateIncomeRateForBracket.Select(x => x).ToArray();
                    break;
                default:
                    Console.WriteLine("Jurisdiction not supported");
                    return 0.00M;
            }

            income = GetAdjustedGrossIncome(status, income);

            // After standard deduction
            var ranges = brackets.Select(bracket => bracket.upperBound - bracket.lowerBound);

            var tax = 0.00f;

            var i = 0;
            while (income > brackets[i].upperBound)
            {
                tax += (float)ranges.ElementAt(i) * rateForBracket[i];
                i++;
            }

            var cherryOnTop = (float)(income - brackets[i].lowerBound - basicAdjustment) * rateForBracket[i];

            var total = tax + cherryOnTop - GetChildTaxCredit(status, Data.NumberOfChildren, income);

            return (decimal)total;
        }

        public static decimal GetAdjustedGrossIncome(FilingStatus status, decimal income)
        {
            income -= GetStandardDeduction(filingStatus: status, jurisdiction: "Federal");
            return income < 0.00M ? 0.00M : income;
        }

        private static decimal GetStandardDeduction(FilingStatus filingStatus, string jurisdiction)
        {
            var standardDeduction = 0.00M;

            switch (jurisdiction)
            {
                case "Federal":
                    switch (filingStatus)
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
                    break;
                case "VA State":
                    switch (filingStatus)
                    {
                        case FilingStatus.Joint:
                            standardDeduction = 6000.00M;
                            break;
                        case FilingStatus.HeadOfHousehold:
                            standardDeduction = 4500.00M;
                            break;
                        case FilingStatus.Unmarried:
                        case FilingStatus.MarriedSeparate:
                        default:
                            standardDeduction = 3000.00M;
                            break;
                    }
                    break;
                case "Capital Gains":
                    break;
                default:
                    Console.WriteLine("Jurisdiction not supported");
                    return 0.00M;
            }
            return standardDeduction;
        }

        private static float GetChildTaxCredit(FilingStatus status, int numberOfChildren, decimal adjustedGrossIncome)
        {
            var childTaxCredit = 2000.00f;

            // Phaseout thresholds
            switch (status)
            {
                case FilingStatus.Joint:
                    if (adjustedGrossIncome >= 400000.00M && adjustedGrossIncome < 440000)
                    {
                        var overage = MathF.Floor(((float)adjustedGrossIncome - 400000.00f) / 1000);
                        childTaxCredit -= 50 * overage;
                    }
                    else if (adjustedGrossIncome >= 440000.00M)
                    {
                        childTaxCredit = 0;
                    }
                    break;
                case FilingStatus.HeadOfHousehold:
                case FilingStatus.Unmarried:
                case FilingStatus.MarriedSeparate:
                default:
                    if (adjustedGrossIncome >= 200000.00M && adjustedGrossIncome < 240000)
                    {
                        var overage = MathF.Floor(((float)adjustedGrossIncome - 200000.00f) / 1000);
                        childTaxCredit -= 50 * overage;
                    }
                    else if (adjustedGrossIncome >= 240000.00M)
                    {
                        childTaxCredit = 0;
                    }
                    break;
            }
            
            return childTaxCredit * numberOfChildren;
        }
    }
}
