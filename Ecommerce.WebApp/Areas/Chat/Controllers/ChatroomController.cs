using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Identity.Chat.Controllers
{
    public class ChatroomController : Controller
    {
        [Area("Chat")]
        [Route("chatroom")]
        public IActionResult Index()
        {
            return View();
        }
    }
}