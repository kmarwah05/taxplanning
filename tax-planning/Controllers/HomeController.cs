using Microsoft.AspNetCore.Mvc;
using tax_planning.Models;
using tax_planning.Models.TaxCalculation;
using tax_planning.Models.Tools;

namespace tax_planning.Controllers
{
    [Produces("application/json")]
    [Route("api/")]
    public class HomeController : ControllerBase
    {
        // POST api/
        [HttpPost]
        public string Post([FromForm] FormResponse response)
        {
            return response.FilingStatus.ToString();
        }

    }
}
