using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Data.Entities
{
    public class District
    {
        public int ID { get; set; }
        public int ProvinceID { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
    }
}
