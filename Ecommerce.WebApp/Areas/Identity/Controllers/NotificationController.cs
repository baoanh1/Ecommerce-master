using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Identity.Areas.Identity.Controllers
{
    public class NotificationController : Controller
    {
        public IActionResult EmailConfirmed(string userID, string token)
        {
            return View();
        }
    }
}