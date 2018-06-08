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
        public List<IAsset> Assets { get; set; }
        
        // POST api/
        [HttpPost]
        public JsonResult Post([FromForm] FormModel response)
        {
            var assetsString = response.FormAssets;

            var assetsStringArray = JsonConvert.DeserializeObject<string[][]>(assetsString);
            foreach (string[] element in assetsStringArray)
            {
                Assets.Add(AssetFactory.Create(
                    name: element[0],
                    assetType: (AssetType)Enum.Parse(typeof(AssetType), element[1]),
                    value: Decimal.Parse(element[2])
                ));
            }

            return Json(response);
        }

    }
}
