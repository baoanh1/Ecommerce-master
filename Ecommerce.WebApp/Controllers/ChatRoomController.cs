using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Application.Services;
using Ecommerce.Core.Models;
using Ecommerce.Data.Entities;
using Ecommerce.Identity.Chat.Hubs;
using Ecommerce.WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SmartBreadcrumbs.Attributes;
using static Ecommerce.WebApp.Models.ChatViewModel;

namespace Ecommerce.WebApp.Controllers
{
    
    public class ChatRoomController : Controller
    {
        private readonly IHubContext<ChatHub> _hubContext;
        IRepository<Product> _productRepository;
        IRepository<ChatRoom> _ChatRoomRepository;
        IRepository<Message> _MessageRepository;
        IUnitOfWork UOW;
        private UserManager<AppUser> _usermanager { get; }
        public ChatRoomController(IUnitOfWork uow, UserManager<AppUser> usermanager, IHubContext<ChatHub> hubContext)
        {
            UOW = uow;
            _productRepository = UOW.GetRepository<Product>();
            _usermanager = usermanager;
            _ChatRoomRepository = UOW.GetRepository<ChatRoom>();
            _MessageRepository = UOW.GetRepository<Message>();
            _hubContext = hubContext;
        }
        [Breadcrumb("ChatRoom", FromAction = "Index", FromController = typeof(HomeController))]
        public IActionResult Room(int productID)
        {
            var product = _productRepository.GetByID(productID);
            var currentUserID = _usermanager.GetUserId(User);
            Guid id = new Guid(currentUserID);
            var productuserid = ((product != null) ? product.UserID.ToString() : "0");
            string userIds = productuserid + "," + id;
            ViewBag.UserIDs = product != null ? userIds:"0";
            ViewBag.productid = (product != null) ? productID:0;
            return View();
        }
        [Route("chatlist/{userids}/{productid}")]
        [Breadcrumb("Chat")]
        public ChatViewModel GetChatList(string userids, int productid=0)
        {
            var arrayuserids = userids.Split(',');
            var chatroom = _ChatRoomRepository.GetAll().Where(x => x.UserIDs == userids).FirstOrDefault();

            //var messages = _ChatRoomRepository.GetAll().Where(x => x.SenderID == id || x.ReceiverID == id).ToList();
            //ViewBag.receive = messages;

            var currentUserID = _usermanager.GetUserId(User);
            Guid id = new Guid(currentUserID);
            //ViewBag.chat = userIds;
            if (chatroom == null && productid != 0)
            {
                var product = _productRepository.GetByID(productid);
                var newchatroom = new ChatRoom
                {
                    UserIDs = userids,
                    SenderID = id,
                    SenderStatus = 1,
                    ProductID = productid,
                    ReceiverStatus = 1,
                    ReceiverID = product.UserID,
                };
                _ChatRoomRepository.Add(newchatroom);
            }
            
            var messread = chatroom != null ? _MessageRepository.GetAll().Where(x => x.ChatRoomID == chatroom.ID).ToList() : new List<Message>();
            if (messread.Count > 0)
            {
                var mestemp = new List<Message>();
                foreach (var item in messread)
                {
                    if (item.Status)
                    {
                        item.Status = false;
                        mestemp.Add(item);
                    }
                }
                _MessageRepository.Update(mestemp);

            }
            UOW.SaveChanges();
            //var currentUser = _usermanager.FindByIdAsync(id).Result;
            var chatlist = ChatViewModel.Get(_ChatRoomRepository, _MessageRepository, id, userids, _usermanager);
            return chatlist;
        }
        [Route("deletechatroom/{chatid}")]
        public async Task<IActionResult> DeleteChatRoom(int chatid)
        {
            await _ChatRoomRepository.Delete(chatid);
            var res = UOW.SaveChanges();
            if (res > 0)
            {
                var str = Ok(GetSuccessMessage("The chat has been deleted."));
                return str;
            }
            return Ok(GetErrorMessage("The chat could not be found."));
        }

        [Route("deletechat/{chatid}")]
        public async Task<ChatRoom> DeleteChat(int chatid)
        {
            var res = 0;
            var chatroom = _ChatRoomRepository.GetByID(chatid);
            var currentUserID = _usermanager.GetUserId(User);
            Guid id = new Guid(currentUserID);
            var product = _productRepository.GetByID(chatroom.ProductID);
            if (chatroom!=null)
            {
                if(id == product.UserID)
                {
                    chatroom.ReceiverStatus = 0;
                }    
                else
                {
                    chatroom.SenderStatus = 0;
                }
                _ChatRoomRepository.Update(chatroom);
                res = UOW.SaveChanges();
                if (res > 0)
                {

                    var chatroom2 = _ChatRoomRepository.GetByID(chatid);
                    return chatroom2;
                }
            }
            return new ChatRoom();
        }
        [Route("SaveMessages")]
        public ChatViewModel SaveMessages([FromBody] ChatViewModel model)
        {
            var currentUserID = _usermanager.GetUserId(User);
            Guid id = new Guid(currentUserID);
            //var chatroom = _ChatRoomRepository.GetAll().Where(x => x.UserIDs == PrivateName).FirstOrDefault();
            
            var mess = new Message
            {
                ChatRoomID = model.Chatroom.ID,
                SenderID = id.ToString(),
                Mes = model.Message.Mes,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };

            _MessageRepository.Add(mess);
            var cusMes = new CusMessage 
            {
                ID = mess.ID,
                ChatRoomID = mess.ChatRoomID,
                Mes = mess.Mes,
                SenderID = mess.SenderID,
                SenderName = _usermanager.FindByIdAsync(mess.SenderID).Result.UserName,
                Status = mess.Status,
                CreatedDate = mess.CreatedDate,
                ModifiedDate = mess.ModifiedDate

            };

            UOW.SaveChanges();
            var myHub = _hubContext.Clients.Group(model.Chatroom.UserIDs).SendAsync("ReceiveMessage", model.Chatroom.UserIDs, cusMes);
            _hubContext.Clients.All.SendAsync("AllReceiveMessage", model.Chatroom.UserIDs, cusMes);
            model.Messages = _MessageRepository.GetAll()
                .OrderBy(m => m.ID).Where(x => x.ChatRoomID == model.Chatroom.ID)
                .Select(m => new CusMessage
                {
                    ID = m.ID,
                    ChatRoomID = m.ChatRoomID,
                    Mes = m.Mes,
                    SenderID = m.SenderID,
                    SenderName = _usermanager.FindByIdAsync(m.SenderID).Result.UserName,
                    Status = m.Status,
                    CreatedDate = m.CreatedDate,
                    ModifiedDate = m.ModifiedDate

                }).ToList();
            //model.Messages = _MessageRepository.GetAll().Where(x => x.ChatRoomID == model.Chatroom.ID).ToList();
            return model;
        }
        private StatusMessage GetSuccessMessage(string message)
        {
            return GetMessage(message, StatusMessage.Success);
        }
        private StatusMessage GetErrorMessage(string message)
        {
            return GetMessage(message, StatusMessage.Error);
        }
        private StatusMessage GetMessage(string message, string type)
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