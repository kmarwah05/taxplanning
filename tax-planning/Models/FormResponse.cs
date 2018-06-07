using System.ComponentModel.DataAnnotations;
using tax_planning.Models.TaxCalculation;

namespace tax_planning.Models
{
    public class FormResponse
    {
        [Required]
        public FilingStatus FilingStatus { get; set; }

        [Required]
        [Range(0, 6, ErrorMessage = "Invalid income bracket")]
        public int IncomeBracket { get; set; }
    }
}
