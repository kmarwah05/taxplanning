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

        public decimal CapitalGains { get; set; }

        public List<IAsset> Assets { get; set; }
    }
}
