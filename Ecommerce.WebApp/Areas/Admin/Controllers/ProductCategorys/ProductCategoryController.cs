using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Application.Services;
using Ecommerce.Application.Services.Paging;
using Ecommerce.Data.Entities;
using Ecommerce.WebApp.Areas.Admin.ProductCategoryModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.WebApp.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("admin")]
    //[Route("admin/[controller]")]

    public class ProductCategoryController : MessageController
    {
        IUnitOfWork UOW;
        IRepository<ProductCategory> _productCategoryRepository;
        public ProductCategoryController(IUnitOfWork uow)
        {
            UOW = uow;
            _productCategoryRepository = uow.GetRepository<ProductCategory>();
        }
        [Route("admin/productcategories")]
        public IActionResult List()
        {
            return View();
        }
        [Route("admin/productcategory/list/{index}/{size}/{from}")]
        public IPaginate<ProductCategoryListModel.ListItem> Get(int index, int size, int from)
        {
            var productList = ProductCategoryListModel.Get(_productCategoryRepository);
            var page = IPaginateExtension.ToPaginate<ProductCategoryListModel.ListItem>(productList.ProductCategories.ToList(), index, size, from);
            return page;
        }
        [Route("admin/productcategory/alllist")]
        public ProductCategoryListModel GetAll()
        {
            var productList = ProductCategoryListModel.Get(_productCategoryRepository);
          
            return productList;
        }
        [Route("/admin/productcategory/add")]
        public ProductCategoryEditModel Add()
        {
            return ProductCategoryEditModel.Create(_productCategoryRepository);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();

        }

        [Route("/admin/productcategory/{id?}")]
        public IActionResult Edit(int id)
        {
            ViewBag.productID = id;
            return View();
        }
        [Route("/admin/productcategory/edit/{id}")]
        public ProductCategoryEditModel Get(int id)
        {
            return ProductCategoryEditModel.GetById(_productCategoryRepository, id);
        }
        [HttpPost]
        [Route("/admin/productcategory/save")]

        public async Task<IActionResult> Save([FromBody] ProductCategoryEditModel model)
        {
            // Refresh roles in the model if validation fails
            //var temp = UserEditModel.Create(_db);
            //model.Roles = temp.Roles;

            if (model.ProductCategory == null)
            {
                return BadRequest("The user could not be found.");
            }



            try
            {
                var productCategoryId = model.ProductCategory.ID;
                var isNew = productCategoryId == 0;

                if (string.IsNullOrWhiteSpace(model.ProductCategory.Name))
                {
                    return BadRequest("Name is mandatory.");
                }

                //if (string.IsNullOrWhiteSpace(model.SelectedProductCategory))
                //{
                //    return BadRequest("product category is mandatory.");
                //}

                var result = model.Save(_productCategoryRepository);
                if (result)
                {
                    var res = UOW.SaveChanges();
                    if (res > 0)
                    {
                        return Ok(Get(model.ProductCategory.ID));
                    }

                }

                var errorMessages = new List<string>();
                return BadRequest("The user could not be saved." + "<br/><br/>" + string.Join("<br />", errorMessages));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("/admin/productcategory/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productCategoryRepository.Delete(id);
            var res = UOW.SaveChanges();
            if (res > 0)
            {
                var str = Ok(GetSuccessMessage("The product has been deleted."));
                return str;
            }
            else
            {
                return Ok(GetErrorMessage("could not be save change."));
            }

            return Ok(GetErrorMessage("The product could not be found."));
        }
        //public ProductEditModel Get(int id)
        //{
        //    var product = _productRepository.GetByID(id);
        //    var productcategories = _productCategoryRepository.GetAll().ToList();
        //    var productcategory = product.Name;
        //    var model = new ProductEditModel
        //    {
        //        Product = product,
        //        ProductCategorys = productcategories,
        //        SelectedProductCategory = productcategory
        //    };

        //    return model;
        //}

    }
}