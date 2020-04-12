using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Data.Entities
{
    public class Receiver
    {
        public int ID { get; set; }
        public int MessageID { get; set; }
        public int ReceiverID { get; set; }
        public DateTime ReadAt { get; set; }
    }
}
