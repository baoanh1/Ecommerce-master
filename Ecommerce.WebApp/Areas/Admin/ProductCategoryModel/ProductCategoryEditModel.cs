using Ecommerce.Application.Services;
using Ecommerce.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.WebApp.Areas.Admin.ProductCategoryModel
{
    public class ProductCategoryEditModel
    {

        public ProductCategory ProductCategory { get; set; }
        public IList<ProductCategory> ProductCategorys { get; set; } = new List<ProductCategory>();
        public static ProductCategoryEditModel Create(IRepository<ProductCategory> ProductCategoryRepository)
        {
            var productCategoryEdit =  new ProductCategoryEditModel
            {
                ProductCategory = new ProductCategory(),
                ProductCategorys = ProductCategoryRepository.GetAll().OrderBy(r => r.Name).ToList(),
            };
            return productCategoryEdit;
        }

        public static ProductCategoryEditModel GetById(IRepository<ProductCategory> ProductCategoryRepository,  int id)
        {
            var productcategory = ProductCategoryRepository.GetByID(id);
            if (productcategory != null)
            {
                var model = new ProductCategoryEditModel
                {
                    ProductCategory = productcategory,
                    ProductCategorys = ProductCategoryRepository.GetAll().OrderBy(r => r.ID).ToList()
                };
                return model;
            }

            return null;
        }

        public bool Save(IRepository<ProductCategory> ProductCategoryRepository)
        {
            bool status = false;
            var productcategory = ProductCategoryRepository.GetByID(ProductCategory.ID);

            if (productcategory == null)
            {

                ProductCategoryRepository.Add(ProductCategory);
                

                status = true;

            }
            else
            {
                productcategory.ID = ProductCategory.ID;
                productcategory.Name = ProductCategory.Name;
                productcategory.Image = ProductCategory.Image;
                productcategory.ParentID = ProductCategory.ParentID;
                ProductCategoryRepository.Update(productcategory);

                status = true;
            }

            return status;
        }
    }
}
