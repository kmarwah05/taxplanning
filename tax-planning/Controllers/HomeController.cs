using System;
using System.Collections.Generic;
using Newtonsoft.Json;
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
            var assetsString = response.FormAssets;
            var assets = new List<IAsset>();

            var assetsResp = JsonConvert.DeserializeObject<string[,]>(assetsString);


            return Json(response);
        }

    }
}
