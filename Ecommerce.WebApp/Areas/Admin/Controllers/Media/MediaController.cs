using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.WebApp.Areas.Admin.Controllers.Media
{
    [Area("admin")]
    public class MediaController : Controller
    {
        [Route("admin/media")]
        public IActionResult list()
        {
            return View();
        }
    }
}