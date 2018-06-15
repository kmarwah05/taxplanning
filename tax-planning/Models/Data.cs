using System;
using System.Collections.Generic;

namespace tax_planning.Models
{
    public class Data
    {
        public static FilingStatus FilingStatus { get; set; }
    
        public static decimal Income { get; set; }

        public static decimal RetirementIncome { get; set; }

        public static decimal BasicAdjustment { get; set; }
    
        public static decimal CapitalGains { get; set; }
    
        public static int RetirementDate { get; set; }
    
        public static int EndOfPlanDate { get; set; }
    
        public static decimal DesiredAdditions { get; set; }

        public static List<Asset> Assets { get; set; } = new List<Asset>();

        private static List<decimal> Additions
        {
            get
            {
                var breakdown = new List<decimal>();
                var total = DesiredAdditions;
                if (total - _401k.MaxContributions <= 0)
                {
                    breakdown.Add(total);
                    breakdown.Add(0);
                    breakdown.Add(0);
                } else if (total - _401k.MaxContributions - Ira.MaxContributions <= 0)
                {
                    breakdown.Add(_401k.MaxContributions);
                    breakdown.Add(total - _401k.MaxContributions);
                    breakdown.Add(0);
                } else
                {
                    breakdown.Add(_401k.MaxContributions);
                    breakdown.Add(Ira.MaxContributions);
                    breakdown.Add(total - _401k.MaxContributions - Ira.MaxContributions);
                }

                return breakdown;
            }
        }

        public static void PopulateData(FormModel formModel) {
            Assets = new List<Asset>();
            // Get data, explicit conversions ok because all nullable fields marked with Required DataAnnotation
            FilingStatus = formModel.FilingStatus.Value;
            Income = formModel.Income.Value;
            BasicAdjustment = formModel.BasicAdjustment;
            CapitalGains = formModel.CapitalGains.Value;
            RetirementDate = formModel.RetirementDate.Value;
            EndOfPlanDate = formModel.EndOfPlanDate.Value;
            DesiredAdditions = formModel.DesiredAdditions.Value;

            // Generate existing assets
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

            // Complete list of assets so client can compare them
            Assets.AddRange(AssetFactory.Complete(Assets));

            // Set optimal additions for each asset
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

            // Picks preferred assets
            Assets.FindAll(asset => asset.AssetType.Equals("Brokerage Holding")).ForEach(holding => holding.Preferred = true);
            var assetPairs = GetAssetPairs();

            assetPairs.ForEach(pair =>
            {
                pair.Item1.Preferred = pair.Item1.AfterTaxWithdrawal > pair.Item2.AfterTaxWithdrawal;
                pair.Item2.Preferred = !pair.Item1.Preferred;
            });

            Assets.FindAll(asset => asset.Preferred).ForEach(asset => RetirementIncome += asset.AfterTaxWithdrawal);
        }

        private static List<(TraditionalRetirementAsset, RothRetirementAsset)> GetAssetPairs()
        {
            List<(TraditionalRetirementAsset, RothRetirementAsset)> pairs = new List<(TraditionalRetirementAsset, RothRetirementAsset)>();

            // 401ks
            List<Asset> _401ks = Assets.FindAll(asset => asset.AssetType.Contains("401k"));
            pairs.Add((_401ks.Find(_401k => !_401k.AssetType.Contains("Roth")) as TraditionalRetirementAsset,
                _401ks.Find(_401k => _401k.AssetType.Contains("Roth")) as RothRetirementAsset));

            // IRAs
            List<Asset> iras = Assets.FindAll(asset => asset.AssetType.Contains("IRA"));
            pairs.Add((iras.Find(ira => !ira.AssetType.Contains("Roth")) as TraditionalRetirementAsset,
                iras.Find(ira => ira.AssetType.Contains("Roth")) as RothRetirementAsset));

            return pairs;
        }
    }
}
