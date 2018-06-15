namespace tax_planning.Models
{
    public class RothIra : RothRetirementAsset
    {
        public static decimal MaxContributions => 5500.00M;

        public RothIra() : base() { }
    }
}
