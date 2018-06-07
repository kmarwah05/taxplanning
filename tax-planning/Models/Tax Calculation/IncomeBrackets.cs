using System;
using System.Collections.Generic;
using tax_planning.Models.Tools;

namespace tax_planning.Models.TaxCalculation
{
    public class IncomeBrackets
    {
        public IEnumerable<Decimal>[] GetBracketsForFilingStatus(FilingStatus status)
        {
            switch (status)
            {
                case FilingStatus.Joint:
                    return new IEnumerable<decimal>[] {
                        Range.Decimal(0.00M, 19050.00M),
                        Range.Decimal(19050.00M, 77400.00M),
                        Range.Decimal(77400.00M, 165000.00M),
                        Range.Decimal(165000.00M, 315000.00M),
                        Range.Decimal(315000.00M, 400000.00M),
                        Range.Decimal(400000.00M, 600000.00M),
                        Range.Decimal(600000.00M, Decimal.MaxValue)
                    };
                case FilingStatus.HeadOfHousehold:
                    return new IEnumerable<decimal>[] {
                        Range.Decimal(0.00M, 13600.00M),
                        Range.Decimal(13600.00M, 51800.00M),
                        Range.Decimal(51800.00M, 82500.00M),
                        Range.Decimal(82500.00M, 157500.00M),
                        Range.Decimal(157500.00M, 200000.00M),
                        Range.Decimal(200000.00M, 500000.00M),
                        Range.Decimal(500000.00M, Decimal.MaxValue)
                    };
                case FilingStatus.Unmarried:
                    return new IEnumerable<decimal>[] {
                        Range.Decimal(0.00M, 9525.00M),
                        Range.Decimal(9525.00M, 38700.00M),
                        Range.Decimal(38700.00M, 82500.00M),
                        Range.Decimal(82500.00M, 157500.00M),
                        Range.Decimal(157500.00M, 200000.00M),
                        Range.Decimal(500000.00M, Decimal.MaxValue)
                    };
                case FilingStatus.MarriedSeparate:
                    return new IEnumerable<decimal>[] {
                        Range.Decimal(0.00M, 9525.00M),
                        Range.Decimal(9525.00M, 38700.00M),
                        Range.Decimal(38700.00M, 82500.00M),
                        Range.Decimal(82500.00M, 157500.00M),
                        Range.Decimal(157500.00M, 300000.00M),
                        Range.Decimal(300000.00M, Decimal.MaxValue)
                    };
                default:
                    throw new ArgumentException("Invalid filing status");
            }
        }
    }
}
