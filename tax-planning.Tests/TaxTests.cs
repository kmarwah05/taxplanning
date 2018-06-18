using System;
using System.Collections.Generic;
using Xunit;
using tax_planning.Models;
using tax_planning.Models.Tax_Calculation;

namespace tax_planning.Tests
{
    public class TaxTests
    {
        public static IEnumerable<object[]> IncomeTaxCases()
        {
            yield return new object[] { FilingStatus.Joint, 142608.00M, 25570.22 };
        }

        [Theory]
        [MemberData(nameof(IncomeTaxCases))]
        public void TestIncomeTax(FilingStatus status, decimal income, decimal expectedTax)
        {
            Assert.Equal(expectedTax, IncomeTaxCalculator.TotalIncomeTaxFor(status, income, 0.00M), 1);
        }
        
    }
}
