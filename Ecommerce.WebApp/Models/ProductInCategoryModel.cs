using Ecommerce.Application.Services;
using Ecommerce.Data.Entities;
using Ecommerce.Data.Enums;
using Ecommerce.WebApp.Extension;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static Ecommerce.WebApp.Extension.Children;

namespace Ecommerce.WebApp.Models
{
    public class ProductInCategoryModel
    {
        public IList<ListProduct> Products { get; set; } = new List<ListProduct>();
        public ListCategory CurentProductCategory { get; set; }
        public IList<Item> ProductCategory { get; set; } = new List<Item>();
        public IList<Item> ProductCategories { get; set; } = new List<Item>();
       
        public static ProductInCategoryModel GetByCategoryID(IUnitOfWork _UOW, IRepository<Product> ProductRepository,
                                           IRepository<ProductCategory> ProductCategoryRepository,
                                           IRepository<ProductImage> productImageRepository,
                                           IRepository<Province> ProvinceRepository,
                                           IRepository<District> DistrictRepository,
                                           IRepository<State> StateRepository, int categoryID)
        {
            var info = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");
            var cate = new Children(_UOW).GetLayers(categoryID);
            var allcate = new Children(_UOW).GetLayers();
            var allcateinparent = new List<int>();
            foreach(var item in cate)
            {
                allcateinparent.Add(item.ID);
                if(item.ChildLayers.Count>0)
                {
                    foreach(var i in item.ChildLayers)
                    {
                        allcateinparent.Add(i.ID);
                    }
                }
            }
            var curentcate = ProductCategoryRepository.GetByID(categoryID);
            var model = new ProductInCategoryModel
            {
                ProductCategory = cate,
                CurentProductCategory = new ListCategory {
                    ID = curentcate.ID,
                    Name = curentcate.Name,
                    Image = curentcate.Image
                },
                Products = ProductRepository.GetAll()
                   .OrderBy(u => u.ID).Where(x => cate.Count>1 ? allcateinparent.Contains(x.categoryID):x.categoryID== categoryID)
                   .Select(u => new ListProduct
                   {
                       ID = u.ID,
                       Name = u.Name,

                       Price = String.Format(info, "{0:N}", u.Price),
                       Images = productImageRepository.GetAll().Where(x => x.ProductID == u.ID).ToList(),
                       categoryName = ProductCategoryRepository.GetByID(u.categoryID).Name,
                       ProvinceName = ProvinceRepository.GetByID(u.ProvinceID).Name,
                       DistrictName = DistrictRepository.GetByID(u.DistrictID).Name,
                       StateName = StateRepository.GetByID(u.StateID).Name,
                       CreatedDate = u.CreatedDate,
                       ModifiedDate = u.ModifiedDate,
                       ModifiedBy = u.ModifiedBy
                   }).ToList(),
                ProductCategories = allcate
            };
            return model;
        }

        public class ListCategory
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Image { get; set; }
            public string MetaTitle { get; set; }

            public int ParentID { get; set; }
            public string ParentName { get; set; }
            public int? DisplayOrder { get; set; }


            public string SeoTitle { get; set; }


            public string MetaKeywords { get; set; }

            public string MetaDescriptions { get; set; }

            public Status Status { get; set; }

            public bool? ShowOnHome { get; set; }
            public IList<ListCategory> ChildLayers { get; set; }
        }
        public class ListProduct
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string UserID { get; set; }
            public string ProductUserName { get; set; }
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
            public DateTime CreatedDate { get; set; }

            public DateTime ModifiedDate { get; set; }
            public string ModifiedBy { get; set; }
        }
    }
}
