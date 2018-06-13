using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tax_planning.Models
{
    public class Data
{
    public FilingStatus FilingStatus { get; set; }
    
    public decimal Income { get; set; }

    public decimal BasicAdjustment { get; set; }
    
    public decimal CapitalGains { get; set; }
    
    public int RetirementDate { get; set; }
    
    public int EndOfPlanDate { get; set; }
    
    public decimal DesiredWithdrawalAmount { get; set; }
    
    public decimal DesiredAdditions { get; set; }

    public List<Asset> Assets { get; } = new List<Asset>();

    public Data(FormModel formModel) {
            FilingStatus = formModel.FilingStatus;
            Income = formModel.Income;
            BasicAdjustment = formModel.BasicAdjustment;
            CapitalGains = formModel.CapitalGains;
            RetirementDate = formModel.RetirementDate;
            EndOfPlanDate = formModel.EndOfPlanDate;
            DesiredWithdrawalAmount = formModel.DesiredWithdrawalAmount;
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
        }
    }
}
