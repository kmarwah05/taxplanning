using System;
using Microsoft.AspNetCore.Mvc;
using tax_planning.Models;

namespace tax_planning.Controllers
{
    [Produces("application/json")]
    [Route("api/")]
    public class HomeController : Controller
    {
        // POST api/
        [HttpPost]
        public JsonResult Post([FromForm] FormModel response)
        {
            return Json(response);
        }

    }
}
