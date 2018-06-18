using System;
using System.Collections.Generic;
using Xunit;
using tax_planning.Models;

namespace tax_planning.Tests
{
    public class AssetTests : Asset
    {
        public static IEnumerable<object[]> FutureValueCases()
        {
            yield return new object[] { 30, 5500M, 434820.02 };
            yield return new object[] { 30, 18500, 1462576.44M };
        }

        [Theory]
        [MemberData(nameof(FutureValueCases))]
        public void TestGetFutureValueAfter(int years, decimal withAdditions, decimal result)
        {
            Assert.Equal(result, GetFutureValueAfter(years, withAdditions), 2);
        }

        public static IEnumerable<object[]> WithdrawalCases()
        {
            yield return new object[] { 434820.02, 30, 31589.20 };
            yield return new object[] { 359982.44, 30, 26152.33 };
            yield return new object[] { 1462576.44, 30, 106254.59 };
            yield return new object[] { 1210850.04, 30, 87966.94 };
            yield return new object[] { 66067.32, 30, 4799.71 };
        }

        [Theory]
        [MemberData(nameof(WithdrawalCases))]
        public void TestGetWithdrawal(decimal initial, int steps, decimal result)
        {
            Assert.Equal(result, GetWithdrawalFor(initial, steps), 1);
        }




        // To satisfy the compiler

        protected override decimal CalculateTaxOnAddition(decimal addition)
        {
            throw new NotImplementedException();
        }

        protected override decimal CalculateTaxOnWithdrawal(decimal withdrawal, decimal income)
        {
            throw new NotImplementedException();
        }
        
    }
}
