namespace tax_planning.Models
{
    public class Ira : TraditionalRetirementAsset
    {
        public override decimal Additions { get => Data.Additions[1]; set => base.Additions = value; }

        public static decimal MaxContributions = 5500.00M;

        public override void UpdateCapsFor(int age)
        {
            if (age >= 50)
            {
                MaxContributions = 6500.00M;
            }
        }
    }
}
