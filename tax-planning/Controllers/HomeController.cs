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
            List<Asset> response = null;

            if (ModelState.IsValid && request != null)
            {
                Data.PopulateData(request);
                response = Data.Assets;
            }
            
            return Json(response);
        }

    }
}
