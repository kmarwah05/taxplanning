using System;
using System.Collections.Generic;

namespace tax_planning.Models
{
    public class Data
    {
        public static FilingStatus FilingStatus { get; set; }
    
        public static decimal Income { get; set; }

        public static decimal BasicAdjustment { get; set; }
    
        public static decimal CapitalGains { get; set; }
    
        public static int RetirementDate { get; set; }
    
        public static int EndOfPlanDate { get; set; }
    
        public static decimal DesiredAdditions { get; set; }

        public static List<decimal> Additions
        {
            get
            {
                var breakdown = new List<decimal>();
                var total = DesiredAdditions;
                if (total - 18500 <= 0)
                {
                    breakdown.Add(total);
                    breakdown.Add(0);
                    breakdown.Add(0);
                } else if (total - 18500 - 5500 <= 0)
                {
                    breakdown.Add(18500);
                    breakdown.Add(total - 18500);
                    breakdown.Add(0);
                } else
                {
                    breakdown.Add(18500);
                    breakdown.Add(5500);
                    breakdown.Add(total - 18500 - 5500);
                }

                return breakdown;
            }
        }
    
        public static List<Asset> Assets { get; set; } = new List<Asset>();

        public static void PopulateData(FormModel formModel) {
            FilingStatus = formModel.FilingStatus;
            Income = formModel.Income;
            BasicAdjustment = formModel.BasicAdjustment;
            CapitalGains = formModel.CapitalGains;
            RetirementDate = formModel.RetirementDate;
            EndOfPlanDate = formModel.EndOfPlanDate;
            DesiredAdditions = formModel.DesiredAdditions;

            foreach (var asset in formModel.Assets)
            {
                try
                {
                    Assets.Add(AssetFactory.Create(
                    name: asset.Name,
                    assetType: asset.Type,
                    value: asset.Value
                ));
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid asset type for asset " + asset.Name + ". Failed to create asset.");
                }
            }

            Assets.AddRange(AssetFactory.Complete(Assets));

            foreach (var asset in Assets)
            {
                if (asset.AssetType.Contains("401k"))
                {
                    asset.Additions = Additions[0];
                }
                else if (asset.AssetType.Contains("IRA"))
                {
                    asset.Additions = Additions[1];
                }
                else
                {
                    asset.Additions = Additions[2];
                }

                asset.CalculateSchedule();
            }
        }
    }
}
