using Ecommerce.Application.Services;
using Ecommerce.Data.Entities;
using Ecommerce.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.WebApp.Areas.Admin.ProductCategoryModel
{
    public class ProductCategoryListModel
    {
        public IList<ListItem> ProductCategories { get; set; } = new List<ListItem>();
        public static ProductCategoryListModel Get(IRepository<ProductCategory> ProductCategoryRepository)
        {

            var model = new ProductCategoryListModel
            {
                ProductCategories = ProductCategoryRepository.GetAll()
                    .OrderBy(u => u.ID)
                    .Select(u => new ListItem
                    {
                        ID = u.ID,
                        Image = u.Image,
                        Name = u.Name,
                        categoryName = u.ParentID==0 ? "Root": ProductCategoryRepository.GetByID(u.ParentID).Name
                    }).ToList()
            };
            return model;
          
        }
        public class ListItem
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Image { get; set; }
            public string MetaTitle { get; set; }

            public int ParentID { get; set; }
            public string categoryName { get; set; }
            public int? DisplayOrder { get; set; }


            public string SeoTitle { get; set; }

          
            public string MetaKeywords { get; set; }

            public string MetaDescriptions { get; set; }

            public Status Status { get; set; }

            public bool? ShowOnHome { get; set; }
        }

    }
}
