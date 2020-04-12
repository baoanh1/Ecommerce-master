using Ecommerce.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Data.Entities
{
    public class ProductCategory
    {

        public int ID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string MetaTitle { get; set; }

        public int ParentID { get; set; }

        public int? DisplayOrder { get; set; }


        public string SeoTitle { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }


        public string ModifiedBy { get; set; }

        public string MetaKeywords { get; set; }

        public string MetaDescriptions { get; set; }

        public Status Status { get; set; }

        public bool? ShowOnHome { get; set; }
    }
}
