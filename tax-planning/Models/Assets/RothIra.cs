namespace tax_planning.Models
{
    public class RothIra : RothRetirementAsset
    {
        public override decimal Additions { get => Data.Additions[1]; set => base.Additions = value; }

        public static decimal MaxContributions => 5500.00M;
    }
}
