using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using tax_planning.Models;

namespace tax_planning.Tests
{
    public class BracketsTests
    {
        [Theory]
        [InlineData(FilingStatus.Joint)]
        public void TestIncomeBracketsForFilingStatus(FilingStatus filingStatus)
        {

        }
    }
}
