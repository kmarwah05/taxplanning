using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace tax_planning.Models
{
    public class FormModel
    {
        [Required]
        public FilingStatus FilingStatus { get; set; }

        [Required]
        public decimal Income { get; set; }

        public decimal BasicAdjustment { get; set; } = 0.00M;

        [Required]
        public int RetirementDate { get; set; }

        [Required]
        public int EndOfPlanDate { get; set; }

        [Required]
        public decimal CapitalGains { get; set; }

        [Required]
        public decimal DesiredWithdrawalAmount { get; set; }

        [Required]
        public decimal DesiredAdditions { get; set; }

        public IEnumerable<AssetModel> Assets { get; set; }
    }
}
