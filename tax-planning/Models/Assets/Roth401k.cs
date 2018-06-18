using System.Linq;

namespace tax_planning.Models
{
    public class Roth401k : RothRetirementAsset
    {
        public static decimal MaxContributions => 18500.00M;

        private _401k Match { get; set; }

        public Roth401k() : base() { }

        public Roth401k((decimal proportion, decimal cap) matching)
        {
            Match = new _401k()
            {
                Additions = Additions += (Additions * matching.proportion > Data.Income * matching.cap) ?
                    Data.Income * matching.cap :
                    Additions * matching.proportion
            };
        }

        public override void CalculateSchedule()
        {
            base.CalculateSchedule();
            Match?.CalculateSchedule();
        }

        public override void CalculateData()
        {
            if (Match != null)
            {
                Match.CalculateData();
                YearlyAmount = YearlyAmount.Select((amount, index) => amount + Match.YearlyAmount[index]).ToList();
                Withdrawal += Match.Withdrawal;
            }

            base.CalculateData();
        }
    }
}
