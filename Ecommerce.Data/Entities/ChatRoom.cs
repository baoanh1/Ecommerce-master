using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Data.Entities
{
    public class ChatRoom
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
       
        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
