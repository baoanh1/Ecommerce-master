using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Identity.Areas.Chat
{
    public static class ChatModuleExtensions
    {
        public static IApplicationBuilder UserChatModule(this IApplicationBuilder builder)
        {
            var res = builder.UseStaticFiles(new StaticFileOptions 
            {
                FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), @"Areas/Chat")),
                RequestPath = new PathString("/chat")
            });
            return res;
        }
    }
}
