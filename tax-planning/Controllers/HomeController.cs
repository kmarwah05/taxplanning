using Microsoft.AspNetCore.Mvc;
using tax_planning.Models;

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
