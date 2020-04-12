using Ecommerce.Application.Services;
using Ecommerce.Data.Entities;
using Ecommerce.Data.Enums;
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
        public IList<Message> Messages { get; set; } = new List<Message>();
        public static ChatViewModel Get(IRepository<ChatRoom> ChatRepository, IRepository<Message> MessageRepository, Guid id, string userids)                          
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
                       ReceiverID = u.ReceiverID
                   }).ToList(),
                Messages = MessageRepository.GetAll().Where(x => x.ChatRoomID == chatroom.ID).ToList(),
                Chatroom = new ListItem()
                {
                    ID = chatroom.ID,
                    UserIDs = chatroom.UserIDs,
                    SenderID = chatroom.SenderID,
                    ReceiverID = chatroom.ReceiverID
                },
                Message = new Message()
            };
            return model;
        }
        
        public class ListItem
        {
            public int ID { get; set; }
            public enum RoomType { Private, Group, Publuc }
            public string UserIDs { get; set; }
            public Guid SenderID { get; set; }
            public Guid ReceiverID { get; set; }
            public int ProductID { get; set; }
            public int SenderStatus { get; set; }
            public int ReceiverStatus { get; set; }
            public enum Action { Buy, Sell }
        }
    }
}
