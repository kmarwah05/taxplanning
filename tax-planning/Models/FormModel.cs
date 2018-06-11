using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using tax_planning.Models;
using tax_planning.Models.Assets;

namespace tax_planning.Models
{
    public class FormModel
    {
        [Required]
        public FilingStatus FilingStatus { get; set; }

        [Required]
        public decimal Income { get; set; }

        [Required]
        public decimal BasicAdjustment { get; set; }

        [Required]
        public decimal CapitalGains { get; set; }

        [Required]
        public DateTime RetirementDate { get; set; }

        [Required]
        public DateTime EndOfPlanDate { get; set; }

        [Required]
        public string FormAssets
        {
            set
            {
                var assetsStringArray = Newtonsoft.Json.JsonConvert.DeserializeObject<string[][]>(value);
                foreach (string[] element in assetsStringArray)
                {
                    try
                    {
                        Assets.Add(AssetFactory.Create(
                        name: element[0],
                        assetType: element[1],
                        value: Decimal.Parse(element[2])
                    ));
                    } catch (Exception)
                    {
                        Console.WriteLine("Invalid asset type for asset " + element[0] + ". Failed to create asset.");
                    }
                }
            }
        }

        public AssetListDecorator Assets { get; } = new AssetListDecorator();
    }
}
