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
        public JsonResult Post(string request)
        {
            Table mockTable1 = new Table()
            {
                YearlyAmount = new List<decimal>()
                {
                    4000M,
                    4000M,
                    4000M,
                    4000M,
                    4000M,
                    4000M,
                    4000M,
                    4000M,
                    4000M,
                    4000M,
                    -35000M,
                    -35000M,
                    -36000M,
                    -38000M,
                    -45000M,
                    -26375.97M
                },
                YearlyChange = new List<decimal>()
                {
                    0M,
                    0M,
                    0M,
                    0M,
                    0M,
                    0M,
                    0M,
                    0M,
                    0M,
                    0M,
                    1500M,
                    1500M,
                    1750M,
                    1833M,
                    2200M,
                    678.32M
                }
            };
            Table mockTable2 = new Table()
            {
                YearlyAmount = new List<decimal>()
                {
                    4000M,
                    4000M,
                    4000M,
                    6000M,
                    6000M,
                    6000M,
                    6000M,
                    7000M,
                    7000M,
                    7000M,
                    -39000M,
                    -39000M,
                    -39000M,
                    -39000M,
                    -39000M,
                    -41233M
                },
                YearlyChange = new List<decimal>()
                {
                    0M,
                    0M,
                    0M,
                    0M,
                    0M,
                    0M,
                    0M,
                    0M,
                    0M,
                    0M,
                    1950M,
                    1950M,
                    1950M,
                    1950M,
                    1950M,
                    2111.31M
                }
            };

            Dictionary<string, Table> mockResponse = new Dictionary<string, Table>()
            {
                { "Overall Optimal", mockTable1 },
                { "Overall Desired", mockTable2 },
                { "401k Optimal", mockTable1 },
                { "401k Desired", mockTable2 }
            };

            return Json(mockResponse);
        }
    }
}