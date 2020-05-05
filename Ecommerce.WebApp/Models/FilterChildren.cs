using Ecommerce.Application.Services;
using Ecommerce.Data.Entities;
using Ecommerce.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.WebApp.Models
{
    public class FilterChildren
    {
        public IList<Item> getChildren { get; set; } = new List<Item>();
        public static FilterChildren Get(IRepository<ProductCategory> ProductCategoryRepository)
        {
            var Categories = ProductCategoryRepository.GetAll()
                   .OrderBy(u => u.ID)
                   .Select(u => new Item
                   {
                       ID = u.ID,
                       Name = u.Name,
                       ParentID = u.ParentID
                   }).ToList();
            var model = new FilterChildren
            {
                   getChildren = GetChildren(Categories, 0)
            };
            return model;
        }

        public static IList<Item> GetChildren(IList<Item> source, int parentId)
        {
            var children = source.OrderBy(u => u.ID).Where(x => x.ParentID == parentId).ToList();
            //GetChildren is called recursively again for every child found
            //and this process repeats until no childs are found for given node, 
            //in which case an empty list is returned
            children.ForEach(x => x.ChildLayers = GetChildren(source, x.ID));
            return children;
        }
        public class Item
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

            public IList<Item> ChildLayers { get; set; }
        }
    }
}
