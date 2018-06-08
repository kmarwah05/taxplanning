using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using tax_planning.Models;

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
        public DateTime RetirementDate { get; set; }

        [Required]
        public decimal CapitalGains { get; set; }

        [Required]
        public string FormAssets { get; set; }

        //public List<IAsset> Assets { get; private set; }

        //public FormModel(
        //    FilingStatus filingStatus,
        //    Decimal income,
        //    Decimal basicAdjustment,
        //    DateTime retirementDate,
        //    DateTime endOfPlanDate,
        //    Decimal capitalGains,
        //    params string[] assets)
        //{
        //    if (assets.Length % 3 != 0)
        //    {
        //        throw new ValidationException("Invalid asset data");
        //    }

        //    for (var i = 0; i < assets.Length; i += 3)
        //    {
        //        Assets.Add(AssetFactory.Create(
        //            name: assets[i],
        //            assetType: (AssetType)Enum.Parse(typeof(AssetType), assets[i + 1]),
        //            value: Decimal.Parse(assets[i + 2])
        //        ));
        //    }
        //}
    }
}
