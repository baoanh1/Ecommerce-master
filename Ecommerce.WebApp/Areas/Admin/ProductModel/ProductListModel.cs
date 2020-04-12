using Ecommerce.Application.Services;
using Ecommerce.Data.Entities;
using Ecommerce.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.WebApp.Areas.Admin.ProductModel
{
    public class ProductListModel
    {
        public IList<ListItem> Products { get; set; } = new List<ListItem>();
        public static ProductListModel Get(IRepository<Product> ProductRepository, IRepository<ProductCategory> ProductCategoryRepository, IRepository<ProductImage> productImageRepository, Guid userid)
        {

            var model = new ProductListModel
            {
                Products = ProductRepository.GetAll()
                    .OrderBy(u => u.ID).Where(x=>x.UserID== userid)
                    .Select(u => new ListItem
                    {
                        ID = u.ID,
                        Name = u.Name,
                        Price = u.Price,

                        Image = productImageRepository.GetAll().Where(x=>x.ProductID==u.ID).FirstOrDefault(),
                        categoryName = ProductCategoryRepository.GetByID(u.categoryID).Name
                    }).ToList()
            };
            return model;

            //var products = ProductRepository.GetAll();
            //IEnumerable<ProductListModel> model = from a in ProductRepository.Query("SELECT * from Product")
            //                                      join b in ProductCategoryRepository.Query("SELECT * from ProductCategory") on a.categoryID equals b.ID
            //                                      select new ProductListModel()
            //                                      {
            //                                          Products = new Product(new ListItem { });
            //                                      };
            //var model = new ProductListModel
            //{
            //    Products = products.OrderBy(p => p.Name).Select(p => new ListItem
            //    {
            //        ID = p.ID,
            //        Name = p.Name,
            //        categoryNames = p.ProductInCategories.Select(x => x.ProductCategory.Name).ToList(),
            //    }).ToList()
            //};
            
            //return model;
        }
        public class ListItem
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public ProductImage Image { get; set; }
            public string Code { get; set; }
            public string MetaTitle { get; set; }
            public string Description { get; set; }

            public decimal Price { get; set; }

            public decimal PromotionPrice { get; set; }

            public int Quantity { get; set; }

            public long categoryID { get; set; }
            public string categoryName { get; set; }
            public string Detail { get; set; }

            public int Warranty { get; set; }

            public string MetaKeywords { get; set; }

            public string MetaDescriptions { get; set; }

            public Status Status { get; set; }

            public bool TopHot { get; set; }

            public int ViewCount { get; set; }
        }

    }
}
