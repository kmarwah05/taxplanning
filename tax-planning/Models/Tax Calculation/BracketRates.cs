using System.Collections.Generic;

namespace tax_planning.Models.TaxCalculation
{
    public class BracketRates
    {
        public static float[] IncomeRateForBracket = new float[] { 0.10f, 0.12f, 0.22f, 0.24f, 0.32f, 0.35f, 0.37f };
        public static float[] CapitalGainsRateForBracket = { 0.10f, 0.15f, 0.20f };
    }
}
