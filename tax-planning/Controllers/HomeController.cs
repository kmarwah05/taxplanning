using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using tax_planning.Models;


namespace tax_planning.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    public class HomeController : Controller
    {
        // Handels POST to /api
        [HttpPost]
        public JsonResult Post([FromBody] FormModel request)
        {
            Dictionary<string, Table> response = null;

            if (ModelState.IsValid && request != null)
            {
                Data data = new Data(request);
                response = Aggregator.GetTablesFor(data);
            }
            
            return Json(response);
        }

    }
}
