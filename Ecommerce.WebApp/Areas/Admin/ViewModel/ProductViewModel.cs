using Ecommerce.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.WebApp.Areas.Admin.ViewModel
{
    public class ProductViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string MetaTitle { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal PromotionPrice { get; set; }

        public int Quantity { get; set; }

        public long categoryID { get; set; }
        public List<string> categoryNames { get; set; }
        public string Detail { get; set; }

        public int Warranty { get; set; }

        public string MetaKeywords { get; set; }

        public string MetaDescriptions { get; set; }

        public Status Status { get; set; }

        public bool TopHot { get; set; }

        public int ViewCount { get; set; }
        

    }
}
