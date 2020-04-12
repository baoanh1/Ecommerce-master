using Ecommerce.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Data.Entities
{
    public class ProductImage
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public int ProductID { get; set; }
        public int DisplayOrder { get; set; }
        public string Caption { get; set; }
        public bool IsDefault { get; set; }
        public int FileSize { get; set; }
        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }


        public string ModifiedBy { get; set; }
    }
}
