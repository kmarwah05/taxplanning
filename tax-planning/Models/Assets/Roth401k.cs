namespace tax_planning.Models
{
    public class Roth401k : RothRetirementAsset
    {
        public static decimal MaxContributions => 18500.00M;

        public Roth401k() : base() { }
    }
}
