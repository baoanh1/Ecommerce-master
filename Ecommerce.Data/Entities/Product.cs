using Ecommerce.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Data.Entities
{
    public class Product
    {
        public int ID { get; set; }
        public Guid UserID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string MetaTitle { get; set; }
        public string Description { get; set; }
        public int ProvinceID { get; set; }
        public int DistrictID { get; set; }
        public decimal Price { get; set; }

        public decimal PromotionPrice { get; set; }

        public int Quantity { get; set; }

        public int categoryID { get; set; }

        public string Detail { get; set; }

        public int Warranty { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public string MetaKeywords { get; set; }

        public string MetaDescriptions { get; set; }

        public Status Status { get; set; }
        public int StateID { get; set; }
        public bool TopHot { get; set; }

        public int ViewCount { get; set; }

    }
}
