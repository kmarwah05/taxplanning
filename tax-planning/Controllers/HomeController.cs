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
            var assetsString = request.FormAssets;
            List<IAsset> assets = new List<IAsset>();

            var assetsStringArray = JsonConvert.DeserializeObject<string[][]>(assetsString);
            foreach (string[] element in assetsStringArray)
            {
                assets.Add(AssetFactory.Create(
                    name: element[0],
                    assetType: element[1],
                    value: Decimal.Parse(element[2])
                ));
            }

            Response response = Calculator.GetOptimalScheduleFor(
                filingStatus: request.FilingStatus,
                income: request.Income,
                basicAdjustment: request.BasicAdjustment,
                capitalGains: request.CapitalGains,
                retirementDate: request.RetirementDate,
                endOfPlan: request.EndOfPlan,
                assets: assets);

            return Json(response);
        }

    }
}
