using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Data.Entities
{
    public class Message
    {
        public int ID { get; set; }
        public int ChatRoomID { get; set; }
        public string SenderID { get; set; }
        public string Mes { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

    }
}
