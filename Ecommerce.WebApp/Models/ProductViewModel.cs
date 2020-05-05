using Ecommerce.Application.Services;
using Ecommerce.Data.Entities;
using Ecommerce.Data.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.WebApp.Models
{
    public class ProductViewModel
    {
        public IList<ListItem> Products { get; set; } = new List<ListItem>();
        public IList<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
        public static ProductViewModel SearchForm(IRepository<Product> ProductRepository,
                                           IRepository<ProductCategory> ProductCategoryRepository,
                                           IRepository<ProductImage> productImageRepository,
                                           IRepository<Province> ProvinceRepository,
                                           IRepository<District> DistrictRepository,
                                           IRepository<State> StateRepository,
                                           UserManager<AppUser> usermanager, string keyword)
        {
            var info = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");

            var model = new ProductViewModel
            {
                Products = ProductRepository.GetAll()
                   .OrderBy(u => u.ID)
                   .Select(u => new ListItem
                   {
                       ID = u.ID,
                       Name = u.Name,
                       UserID = u.UserID.ToString(),
                       ProductUserName = usermanager.FindByIdAsync(u.UserID.ToString()).Result.UserName,
                       Price = String.Format(info, "{0:c}", u.Price),
                       Images = productImageRepository.GetAll().Where(x => x.ProductID == u.ID).ToList(),
                       categoryName = ProductCategoryRepository.GetByID(u.categoryID).Name,
                       ProvinceName = ProvinceRepository.GetByID(u.ProvinceID).Name,
                       DistrictName = DistrictRepository.GetByID(u.DistrictID).Name,
                       StateName = StateRepository.GetByID(u.StateID).Name
                   }).ToList(),
                ProductCategories = ProductCategoryRepository.GetAll().ToList()
            };
            return model;
        }
        public static ProductViewModel Get(IRepository<Product> ProductRepository,
                                           IRepository<ProductCategory> ProductCategoryRepository,
                                           IRepository<ProductImage> productImageRepository,
                                           IRepository<Province> ProvinceRepository,
                                           IRepository<District> DistrictRepository,
                                           IRepository<State> StateRepository,
                                           UserManager<AppUser> usermanager)
        {
            var info = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");
           
            var model = new ProductViewModel
            {
                Products = ProductRepository.GetAll()
                   .OrderBy(u => u.ID)
                   .Select(u => new ListItem
                   {
                       ID = u.ID,
                       Name = u.Name,
                       UserID = u.UserID.ToString(),
                       ProductUserName = usermanager.FindByIdAsync(u.UserID.ToString()).Result.UserName,
                       Price = String.Format(info, "{0:c}", u.Price),
                       Images = productImageRepository.GetAll().Where(x => x.ProductID == u.ID).ToList(),
                       categoryName = ProductCategoryRepository.GetByID(u.categoryID).Name,
                       ProvinceName = ProvinceRepository.GetByID(u.ProvinceID).Name,
                       DistrictName = DistrictRepository.GetByID(u.DistrictID).Name,
                       StateName = StateRepository.GetByID(u.StateID).Name
                   }).ToList(),
                ProductCategories = ProductCategoryRepository.GetAll().ToList()
            };
            return model;
        }
        public static ProductViewModel GetByCategoryID(IRepository<Product> ProductRepository,
                                           IRepository<ProductCategory> ProductCategoryRepository,
                                           IRepository<ProductImage> productImageRepository,
                                           IRepository<Province> ProvinceRepository,
                                           IRepository<District> DistrictRepository,
                                           IRepository<State> StateRepository, int categoryID)
        {
            var info = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");

            var model = new ProductViewModel
            {
                Products = ProductRepository.GetAll()
                   .OrderBy(u => u.ID).Where(x=>x.categoryID== categoryID)
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
                   }).ToList()
            };
            return model;
        }

        public class ListItem
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
        }
    }
}
