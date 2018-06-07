using System;
using System.Collections.Generic;
using tax_planning.Models.Tools;

namespace tax_planning.Models.TaxCalculation
{
    public class IncomeBrackets
    {
        public static (decimal, decimal)[] GetBracketsForFilingStatus(FilingStatus status)
        {
            switch (status)
            {
                case FilingStatus.Joint:
                    return new (decimal, decimal)[] {
                        (0.00M, 19050.00M),
                        (19050.00M, 77400.00M),
                        (77400.00M, 165000.00M),
                        (165000.00M, 315000.00M),
                        (315000.00M, 400000.00M),
                        (400000.00M, 600000.00M),
                        (600000.00M, Decimal.MaxValue)
                    };
                case FilingStatus.HeadOfHousehold:
                    return new (decimal, decimal)[] {
                        (0.00M, 13600.00M),
                        (13600.00M, 51800.00M),
                        (51800.00M, 82500.00M),
                        (82500.00M, 157500.00M),
                        (157500.00M, 200000.00M),
                        (200000.00M, 500000.00M),
                        (500000.00M, Decimal.MaxValue)
                    };
                case FilingStatus.Unmarried:
                    return new (decimal, decimal)[] {
                        (0.00M, 9525.00M),
                        (9525.00M, 38700.00M),
                        (38700.00M, 82500.00M),
                        (82500.00M, 157500.00M),
                        (157500.00M, 200000.00M),
                        (500000.00M, Decimal.MaxValue)
                    };
                case FilingStatus.MarriedSeparate:
                    return new (decimal, decimal)[] {
                        (0.00M, 9525.00M),
                        (9525.00M, 38700.00M),
                        (38700.00M, 82500.00M),
                        (82500.00M, 157500.00M),
                        (157500.00M, 300000.00M),
                        (300000.00M, Decimal.MaxValue)
                    };
                default:
                    throw new ArgumentException("Invalid filing status");
            }
        }
    }
}
