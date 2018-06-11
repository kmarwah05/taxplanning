using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace tax_planning.Controllers
{
    [Produces("text/plain")]
    [Route("api/test")]
    public class TestController : Controller
    {
        [HttpPost]
        public string Post(string request)
        {
            return request;
        }
    }
}