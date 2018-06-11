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
        public decimal CapitalGains { get; set; }

        [Required]
        public DateTime RetirementDate { get; set; }

        [Required]
        public DateTime EndOfPlan { get; set; }

        [Required]
        public string FormAssets { get; set; }
    }
}
