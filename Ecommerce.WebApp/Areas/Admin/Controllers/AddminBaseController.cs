using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.WebApp.Areas.Admin.Controllers
{
    [Authorize]
    public class AddminBaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}