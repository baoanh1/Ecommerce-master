using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Application.Services;
using Ecommerce.Data.Entities;
using Ecommerce.Identity.Chat.Hubs;
using Ecommerce.WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Ecommerce.WebApp.Controllers
{
    
    public class ChatRoomController : Controller
    {
        //private readonly IHubContext<ChatHub> _hubContext;
        IRepository<Product> _productRepository;
        IRepository<ChatRoom> _ChatRoomRepository;
        IRepository<Message> _MessageRepository;
        IUnitOfWork UOW;
        private UserManager<AppUser> _usermanager { get; }
        public ChatRoomController(IUnitOfWork uow, UserManager<AppUser> usermanager)
        {
            UOW = uow;
            _productRepository = UOW.GetRepository<Product>();
            _usermanager = usermanager;
            _ChatRoomRepository = UOW.GetRepository<ChatRoom>();
            _MessageRepository = UOW.GetRepository<Message>();
        }
        public IActionResult Room(int productID)
        {
            var product = _productRepository.GetByID(productID);
            var currentUserID = _usermanager.GetUserId(User);
            Guid id = new Guid(currentUserID);

            string userIds = product.UserID.ToString() + "," + id;
            ViewBag.UserIDs = userIds;
            ViewBag.productid = productID;
            return View();
        }
        [Route("chatlist/{userids}/{productid}")]
        public ChatViewModel GetChatList(string userids, int productid=0)
        {
            var chatroom = _ChatRoomRepository.GetAll().Where(x => x.UserIDs == userids).FirstOrDefault();

            //var messages = _ChatRoomRepository.GetAll().Where(x => x.SenderID == id || x.ReceiverID == id).ToList();
            //ViewBag.receive = messages;
            
            var currentUserID = _usermanager.GetUserId(User);
            Guid id = new Guid(currentUserID);
            //ViewBag.chat = userIds;
            if (chatroom == null)
            {
                var product = _productRepository.GetByID(productid);
                var newchatroom = new ChatRoom
                {
                    UserIDs = userids,
                    SenderID = id,
                    ReceiverID = product.UserID,
                };
                _ChatRoomRepository.Add(newchatroom);
                UOW.SaveChanges();
            }
            //var currentUser = _usermanager.FindByIdAsync(id).Result;
            var chatlist = ChatViewModel.Get(_ChatRoomRepository, _MessageRepository, id, userids);
            
            return chatlist;
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
                Mes = model.Message.Mes
            };

            _MessageRepository.Add(mess);
            UOW.SaveChanges();
            model.Messages = _MessageRepository.GetAll().Where(x => x.ChatRoomID == model.Chatroom.ID).ToList();
            return model;
        }
        public IActionResult Index()
        {
            return View();
        }

    }
}