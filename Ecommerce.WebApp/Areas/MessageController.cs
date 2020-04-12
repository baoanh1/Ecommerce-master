using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.WebApp.Areas
{
    public class MessageController : Controller
    {
        protected StatusMessage GetSuccessMessage(string message)
        {
            return GetMessage(message, StatusMessage.Success);
        }
        protected StatusMessage GetErrorMessage(string message)
        {
            return GetMessage(message, StatusMessage.Error);
        }
        protected StatusMessage GetMessage(string message, string type)
        {
            var Status = new StatusMessage
            {
                Type = type,
                Body = message
            };
            return Status;
        }
    }
}