namespace tax_planning.Models
{
    public class Ira : TraditionalRetirementAsset
    {
        public override decimal Additions { get => Data.Additions[1]; set => base.Additions = value; }

        public static decimal MaxContributions => 5500.00M;

        protected override void UpdateCapsFor(int age)
        {
        }
    }
}
