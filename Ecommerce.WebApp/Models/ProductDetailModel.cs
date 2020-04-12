using Ecommerce.Application.Services;
using Ecommerce.Data.Entities;
using Ecommerce.Data.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.WebApp.Models
{
    public class ProductDetailModel
    {
        public ListItem Product { get; set; }
        public IList<ListItem> RelateProducts { get; set; } = new List<ListItem>();
        public IList<ProductCategory> ProductCategorys { get; set; } = new List<ProductCategory>();
        public static ProductDetailModel GetByID(IRepository<Product> ProductRepository,
                                           IRepository<ProductCategory> ProductCategoryRepository,
                                           IRepository<ProductImage> productImageRepository,
                                           IRepository<Province> ProvinceRepository,
                                           IRepository<District> DistrictRepository,
                                           IRepository<State> StateRepository, int id)
        {
            var info = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");
            var product = ProductRepository.GetByID(id);
            var model = new ProductDetailModel
            {
                Product = new ListItem
                {
                    ID = product.ID,
                    Name = product.Name,

                    Price = String.Format(info, "{0:c}", product.Price),
                    Images = productImageRepository.GetAll().Where(x => x.ProductID == product.ID).ToList(),
                    categoryName = ProductCategoryRepository.GetByID(product.categoryID).Name,
                    ProvinceName = ProvinceRepository.GetByID(product.ProvinceID).Name,
                    DistrictName = DistrictRepository.GetByID(product.DistrictID).Name,
                    StateName = StateRepository.GetByID(product.StateID).Name
                },
                RelateProducts = ProductRepository.GetAll()
                   .OrderBy(u => u.ID).Where(x => x.categoryID == product.categoryID)
                   .Select(u => new ListItem
                   {
                       ID = u.ID,
                       Name = u.Name,

                       Price = String.Format(info, "{0:c}", u.Price),
                       Images = productImageRepository.GetAll().Where(x => x.ProductID == u.ID).ToList(),
                       categoryName = ProductCategoryRepository.GetByID(u.categoryID).Name,
                       ProvinceName = ProvinceRepository.GetByID(u.ProvinceID).Name,
                       DistrictName = DistrictRepository.GetByID(u.DistrictID).Name,
                       StateName = StateRepository.GetByID(u.StateID).Name
                   }).ToList(),
                ProductCategorys = ProductCategoryRepository.GetAll().ToList()
            };
            return model;
        }
        
        public class ListItem
        {
            public int ID { get; set; }
            public string UserName { get; set; }
            public string Name { get; set; }
            public List<ProductImage> Images { get; set; }
            public string Code { get; set; }
            public string MetaTitle { get; set; }
            public string Description { get; set; }

            public string Price { get; set; }

            public decimal PromotionPrice { get; set; }

            public int Quantity { get; set; }

            public long categoryID { get; set; }
            public string ProvinceName { get; set; }
            public string DistrictName { get; set; }
            public string StateName { get; set; }
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
