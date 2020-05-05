using Ecommerce.Application.Services;
using Ecommerce.Data.Entities;
using Ecommerce.Data.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.WebApp.Models
{
    public class ChatViewModel
    {
        public ListItem Chatroom { get; set; }
        public Message Message { get; set; }
        public IList<ListItem> Chats { get; set; } = new List<ListItem>();
        public IList<CusMessage> Messages { get; set; } = new List<CusMessage>();
        public static ChatViewModel Get(IRepository<ChatRoom> ChatRepository, IRepository<Message> MessageRepository, Guid id, string userids, UserManager<AppUser> usermanager)                          
        {
            var chatroom = ChatRepository.GetAll().Where(x => x.UserIDs == userids).FirstOrDefault();

            var model = new ChatViewModel
            {
                Chats = ChatRepository.GetAll()
                   .OrderBy(u => u.ID).Where(x => x.SenderID == id || x.ReceiverID == id)
                   .Select(u => new ListItem
                   {
                       ID = u.ID,
                       UserIDs = u.UserIDs,
                       SenderID = u.SenderID,
                       ReceiverID = u.ReceiverID,
                       SenderStatus = u.SenderStatus,
                       ReceiverStatus = u.ReceiverStatus,
                       Sender = u.SenderID!=null ? usermanager.FindByIdAsync(u.SenderID.ToString()).Result.UserName: "empty",
                       Receiver = u.ReceiverID!=null ? usermanager.FindByIdAsync(u.ReceiverID.ToString()).Result.UserName:"empty",
                       UnReaded = MessageRepository.GetAll().Where(x => x.ChatRoomID == u.ID && x.Status == true).Count()
                   }).ToList(),
                Messages = chatroom!=null ? MessageRepository.GetAll()
                .OrderBy(m => m.ID).Where(x => x.ChatRoomID == chatroom.ID)
                .Select(m => new CusMessage
                {
                    ID = m.ID,
                    ChatRoomID = m.ChatRoomID,
                    Mes =m.Mes,
                    SenderID = m.SenderID,
                    SenderName = usermanager.FindByIdAsync(m.SenderID).Result.UserName,
                    Status =m.Status,
                    CreatedDate= m.CreatedDate,
                    ModifiedDate = m.ModifiedDate

                }).ToList() : new List<CusMessage>(),
                Chatroom = chatroom != null ? new ListItem()
                {
                    ID = chatroom.ID,
                    UserIDs = chatroom.UserIDs,
                    SenderID = chatroom.SenderID,
                    ReceiverID = chatroom.ReceiverID,
                    Sender = usermanager.FindByIdAsync(chatroom.SenderID.ToString()).Result.UserName,
                    Receiver = usermanager.FindByIdAsync(chatroom.ReceiverID.ToString()).Result.UserName
                } : new ListItem(),
                Message = new Message()
            };
            return model;
        }
        public static ChatViewModel GetList(IRepository<ChatRoom> ChatRepository, IRepository<Message> MessageRepository, Guid id, UserManager<AppUser> usermanager)
        {
            var model = new ChatViewModel
            {
                Chats = ChatRepository.GetAll()
                   .OrderBy(u => u.ID).Where(x => x.SenderID == id || x.ReceiverID == id)
                   .Select(u => new ListItem
                   {
                       ID = u.ID,
                       UserIDs = u.UserIDs,
                       SenderID = u.SenderID,
                       ReceiverID = u.ReceiverID,
                       Sender = usermanager.FindByIdAsync(u.SenderID.ToString()).Result.UserName,
                       Receiver = usermanager.FindByIdAsync(u.ReceiverID.ToString()).Result.UserName,
                       UnReaded = MessageRepository.GetAll().Where(x => x.ChatRoomID == u.ID && x.Status == true).Count()
                   }).ToList(),
            };
            return model;
        }
        public class ListItem
        {
            public int ID { get; set; }
            public enum RoomType { Private, Group, Publuc }
            public string UserIDs { get; set; }
            public Guid SenderID { get; set; }
            public string Sender { get; set; }
            public string Receiver { get; set; }
            public int UnReaded { get; set; }
            public Guid ReceiverID { get; set; }
            public int ProductID { get; set; }
            public int SenderStatus { get; set; }
            public int ReceiverStatus { get; set; }
            public enum Action { Buy, Sell }
        }
        public class CusMessage
        {
            public int ID { get; set; }
            public int ChatRoomID { get; set; }
            public string SenderID { get; set; }
            public string SenderName { get; set; }
            public string Mes { get; set; }
            public bool Status { get; set; }
            public DateTime CreatedDate { get; set; }

            public DateTime ModifiedDate { get; set; }

        }
    }
}
