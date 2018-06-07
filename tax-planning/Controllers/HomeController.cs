using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            return response.IncomeBracket?.ToString() ?? "No arguments";
        }
        
    }
}
