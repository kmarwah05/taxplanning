namespace tax_planning.Models
{
    public class _401k : TraditionalRetirementAsset
    {
        public static decimal MaxContributions => 18500.00M;

        public _401k() : base() { }
    }
}
