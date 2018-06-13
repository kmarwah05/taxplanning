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
            Data data = new Data(request);
            Dictionary<string, Table> response = Aggregator.GetTablesFor(data);
            return Json(response);
        }

    }
}
