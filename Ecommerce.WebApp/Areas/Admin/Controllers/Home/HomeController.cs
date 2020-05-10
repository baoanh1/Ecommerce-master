using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.WebApp.Areas.Admin.Controllers.Home
{
    public class HomeController : Controller
    {
        [Authorize(Roles = "admin")]
        [Area("admin")]
        [Route("admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}