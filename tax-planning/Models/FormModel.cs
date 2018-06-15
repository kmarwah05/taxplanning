using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace tax_planning.Models
{
    public class FormModel
    {
        [Required]
        [EnumDataType(typeof(FilingStatus))]
        public FilingStatus? FilingStatus { get; set; }

        [Required]
        [Range(0, 1000000000000)]
        public decimal? Income { get; set; }

        [Range(0, 1000000000000)]
        public decimal BasicAdjustment { get; set; }

        [Required]
        [Range(1900, 2200)]
        public int? RetirementDate { get; set; }

        [Required]
        [Range(1900, 2200)]
        public int? EndOfPlanDate { get; set; }

        [Required]
        [Range(0, 1000000000000)]
        public decimal? CapitalGains { get; set; }

        [Required]
        [Range(0, 1000000000000)]
        public decimal? DesiredWithdrawalAmount { get; set; }

        [Required]
        [Range(0, 1000000000000)]
        public decimal? DesiredAdditions { get; set; }

        public IEnumerable<AssetModel> Assets { get; set; }
    }
}
