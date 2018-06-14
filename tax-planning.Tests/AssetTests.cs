using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using tax_planning.Models;
using System.Collections;

namespace tax_planning.Tests
{
    public class AssetTests : Asset
    {
       

        // Test data
        public static IEnumerable<object[]> DataForFutureValues()
        {
            yield return new object[] { 12, 3000M, 35000M, 121036.70M };
            yield return new object[] { 12, 10000M, 65000M, 299492.18M };
        }

        public static IEnumerable<object[]> DataForRootFinder()
        {
            yield return new object[] { new Func<double, double>(x => x * x), new Func<double, double>(x => 2 * x), 1.0f, 0.0f };
            yield return new object[] { new Func<double, double>(x => 1 - x), new Func<double, double>(x => -1.0f), 5.0f, 1.0f };
        }

        public static IEnumerable<object[]> DataForGetDelta()
        {
            yield return new object[] { -10000.00M, 114662.90M, 0.0f, 23, 9319.63M };
            yield return new object[] { -20000.00M, 278246.18M, 0.0f, 23, 22615.43M };
        }

        [Theory]
        [MemberData(nameof(DataForFutureValues))]
        public void TestGetFutureValueAfter(int years, decimal startingFrom, decimal withAdditions, decimal result)
        {
            Assert.Equal(result, GetFutureValueAfter(years, withAdditions, startingFrom), 2);
        }

        [Theory]
        [MemberData(nameof(DataForRootFinder))]
        public void TestApproximateRoot(Func<double, double> f, Func<double, double> Df, double guess, double result)
        {
            Assert.Equal(result, ApproximateRoot(f, Df, guess), 0);
        }
        
        [Theory]
        [MemberData(nameof(DataForGetDelta))]
        public void TestGetDeltaFor(double guess, double initial, double final, int steps, decimal result)
        {
            Assert.Equal(result, GetDeltaFor(guess, initial, final, steps), 2);
        }


        // Not for testing
        protected override decimal CalculateTaxOn()
        {
            throw new NotImplementedException("This is a test class. Peddle your wares elsewhere.");
        }
    }
}
