using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        // /Home
        [HttpGet]
        public IActionResult Index()
        {
            System.Diagnostics.Debug.WriteLine("Home Page");
            return View();
        }
    }
}