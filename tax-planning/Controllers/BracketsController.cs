using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using tax_planning.Models;

namespace tax_planning.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class BracketsController : Controller
    {
        // POST api/Brackets
        [HttpPost]
        public JsonResult Post(FilingStatus status)
        {
            IEnumerable<string[]> toReturn = TaxBrackets.GetIncomeBracketsForFilingStatus(status).Select(range => new string[]
            {
                range.Item1.ToString(),
                range.Item2.ToString()
            });

            return Json(toReturn);
        }
    }
}
