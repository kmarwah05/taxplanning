using System;
using System.Collections.Generic;
using Extensions;
using tax_planning.Models;

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

        public static int NumberOfChildren { get; set; }

        public static List<Asset> Assets { get; set; } = new List<Asset>();

        // [0] additions for 401k, [1] additions for IRA, [2] leftovers go in equity
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
            NumberOfChildren = formModel.NumberOfChildren;

            // Generate existing assets
            foreach (var asset in formModel.Assets)
            {
                try
                {
                    Assets.Add(AssetFactory.Create(
                    name: asset.Name,
                    assetType: asset.Type,
                    value: asset.Value,
                    matching: (asset.EmployerMatchPercentage, asset.EmployerMatchCap)
                ));
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid asset type for asset " + asset.Name + ". Failed to create asset.");
                }
            }

            // Complete list of assets so client can compare them
            Assets.AddRange(AssetFactory.Complete(Assets));

            Assets = Assets.SortAssets();

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

            // Calculates tax information
            Assets.FindAll(asset => asset.Preferred).ForEach(asset => RetirementIncome += asset.Withdrawal);
            Assets.ForEach(asset => asset.CalculateData());
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

namespace Extensions
{
    public static class AssetListExtensions
    {
        public static List<Asset> SortAssets(this List<Asset> unsortedList)
        {
            List<Asset> sortedList = new List<Asset>();

            // Look at this elegant sorting algorithm >>>

            sortedList.Add(unsortedList.Find(asset => asset.AssetType.Equals("Roth 401k")));
            sortedList.Add(unsortedList.Find(asset => asset.AssetType.Equals("401k")));
            sortedList.Add(unsortedList.Find(asset => asset.AssetType.Equals("Roth IRA")));
            sortedList.Add(unsortedList.Find(asset => asset.AssetType.Equals("IRA")));
            sortedList.Add(unsortedList.Find(asset => asset.AssetType.Equals("Brokerage Holding")));

            return sortedList;
        }
    }
}