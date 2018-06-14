using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tax_planning.Models;

namespace tax_planning.Controllers
{
    [Produces("application/json")]
    [Route("api/test")]
    public class TestController : Controller
    {
        [HttpPost]
        public JsonResult Post([FromBody] FormModel request)
        {
         
            return Json(request);
        }
    }
}