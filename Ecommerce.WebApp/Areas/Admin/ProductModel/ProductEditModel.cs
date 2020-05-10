using Ecommerce.Application.Services;
using Ecommerce.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.WebApp.Areas.Admin.ProductModel
{
    public class ProductEditModel
    {

        public Product Product { get; set; }
        public IList<Province> Provinces { get; set; } = new List<Province>();
        public IList<District> Districts { get; set; } = new List<District>();
        public IList<State> States { get; set; } = new List<State>();
        public IList<ProductCategory> ProductCategorys { get; set; } = new List<ProductCategory>();
        public IList<ProductImage> ProductImages {get;set;} = new List<ProductImage>();
        public List<string> ProductImagePath { get; set; } = new List<string>();
        public string SelectedProductCategory { get; set; }
        public static ProductEditModel Create(IRepository<Product> ProductRepository, IRepository<ProductCategory> ProductCategoryRepository, IRepository<ProductImage> ProductImageRepository, IRepository<Province> ProvinceRepository, IRepository<District> DistrictRepository, IRepository<State> StateRepository)
        {
            var productEdit = new ProductEditModel
            {
                Product = new Product(),
                ProductCategorys = ProductCategoryRepository.GetAll().OrderBy(r => r.Name).ToList(),
                Provinces = ProvinceRepository.GetAll().ToList(),
                Districts = DistrictRepository.GetAll().ToList(),
                States = StateRepository.GetAll().ToList()
            };
            return productEdit;
        }

        public static ProductEditModel GetById(IRepository<Product> ProductRepository, IRepository<ProductCategory> ProductCategoryRepository, IRepository<ProductImage> ProductImageRepository, IRepository<Province> ProvinceRepository, IRepository<District> DistrictRepository, IRepository<State> StateRepository, int id)
        {
            var product = ProductRepository.GetByID(id);
            var productImages = ProductImageRepository.GetAll().Where(x=>x.ProductID == product.ID).ToList();
            if (product != null)
            {
                var model = new ProductEditModel
                {
                    Product = product,
                    ProductCategorys = ProductCategoryRepository.GetAll().OrderBy(r => r.ID).ToList(),
                    ProductImages = productImages,
                    Provinces = ProvinceRepository.GetAll().ToList(),
                    Districts = DistrictRepository.GetAll().ToList(),
                    States = StateRepository.GetAll().ToList()
                };

                var Productcate = ProductCategoryRepository.GetByID(product.categoryID).Name;
                model.SelectedProductCategory = Productcate;
                return model;
            }

            return null;
        }

        public bool Save(IRepository<Product> ProductRepository, IRepository<ProductCategory> ProductCategoryRepository, IRepository<ProductImage> ProductImageRepository, Guid userid)
        {
            bool status = false;
            var product = ProductRepository.GetByID(Product.ID);

            if (product == null)
            {
                Product.UserID = userid;
                Product.CreatedDate = DateTime.Now;
                ProductRepository.Add(Product);
                

                status = true;

            }
            else
            {
                product.ID = Product.ID;
                product.UserID = userid;
                product.Name = Product.Name;
                product.categoryID = Product.categoryID;
                product.Description = Product.Description;
                product.Detail = Product.Detail;
                product.Price = Product.Price;
                product.PromotionPrice = Product.PromotionPrice;
                product.Quantity = Product.Quantity;
                ProductRepository.Update(product);

                status = true;
            }

            return status;
        }
    }
}
