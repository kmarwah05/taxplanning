using System;
using System.Collections.Generic;
using Newtonsoft.Json;
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
        public JsonResult Post([FromForm] FormModel request)
        {
            Dictionary<string, Table> response = Calculator.GetTablesFor(request);
            return Json(response);
        }

    }
}
