using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Application.Services;
using Ecommerce.Application.Services.Paging;
using Ecommerce.Data.EF;
using Ecommerce.Data.Entities;
using Ecommerce.WebApp.Areas.Admin.ProductModel;
using Ecommerce.WebApp.Areas.Admin.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebApp.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("admin")]
    //[Route("admin/[controller]")]
    //[Route("admin")]
    public class ProductController : MessageController
    {
        IUnitOfWork UOW;
        IRepository<Product> _productRepository;
        IRepository<ProductCategory> _productCategoryRepository;
        IRepository<ProductImage> _productImageRepository;
        IRepository<Province> _provinceRepository;
        IRepository<District> _districtRepository;
        IRepository<State> _StateRepository;
        IHostingEnvironment _env;
        private UserManager<AppUser> _usermanager { get; }
        public ProductController(IUnitOfWork uow, UserManager<AppUser> usermanager, IHostingEnvironment env)
        {
            UOW = uow;
            _productRepository = UOW.GetRepository<Product>();
            _productCategoryRepository = UOW.GetRepository<ProductCategory>();
            _productImageRepository = UOW.GetRepository<ProductImage>();
            _provinceRepository = UOW.GetRepository<Province>();
            _districtRepository = UOW.GetRepository<District>();
            _StateRepository = UOW.GetRepository<State>();
            _usermanager = usermanager;
            _env = env;
        }
        [Route("admin/products")]
        public IActionResult List()
        {
            return View();
        }
        [Route("admin/product/list/{index}/{size}/{from}")]
        public IPaginate<ProductListModel.ListItem> Get(int index, int size, int from)
        {
            var currentUserID = _usermanager.GetUserId(User);
            Guid id = new Guid(currentUserID);
            var productList = ProductListModel.Get(_productRepository, _productCategoryRepository, _productImageRepository, id);
            var page = IPaginateExtension.ToPaginate<ProductListModel.ListItem>(productList.Products.ToList(), index, size, from);
            return page;
        }
        [Route("admin/product/alllist")]
        public ProductListModel Get()
        {
            var currentUserID = _usermanager.GetUserId(User);
            Guid id = new Guid(currentUserID);
            var productList = ProductListModel.Get(_productRepository, _productCategoryRepository, _productImageRepository,id);
         
            return productList;
        }
        [Route("/admin/product/add")]
        public ProductEditModel Add()
        {
            var root = _env.WebRootPath;
            var currentUserID = _usermanager.GetUserId(User);
            string path = root + "\\uploads\\" + currentUserID;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return ProductEditModel.Create(_productRepository, _productCategoryRepository, _productImageRepository, _provinceRepository, _districtRepository, _StateRepository);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.productID = 0;
            return View();

        }
       
        [Route("/admin/product/{id?}")]
        public IActionResult Edit(int id)
        {
            ViewBag.productID = id;
            return View();
        }
        [Route("/admin/product/edit/{id}")]
        public ProductEditModel Get(int id)
        {
            return ProductEditModel.GetById(_productRepository, _productCategoryRepository, _productImageRepository, _provinceRepository, _districtRepository, _StateRepository, id);
        }
        [HttpPost]
        [Route("/admin/product/save")]

        public async Task<IActionResult> Save([FromBody] ProductEditModel model)
        {
            // Refresh roles in the model if validation fails
            //var temp = UserEditModel.Create(_db);
            //model.Roles = temp.Roles;

            if (model.Product == null)
            {
                return BadRequest("The user could not be found.");
            }



            try
            {
                var productId = model.Product.ID;
                var isNew = productId == 0;

                if (string.IsNullOrWhiteSpace(model.Product.Name))
                {
                    return BadRequest("Name is mandatory.");
                }

                //if (string.IsNullOrWhiteSpace(model.SelectedProductCategory))
                //{
                //    return BadRequest("product category is mandatory.");
                //}
                var currentUserID = _usermanager.GetUserId(User);
                Guid id = new Guid(currentUserID);
                var result = model.Save(_productRepository, _productCategoryRepository, _productImageRepository, id);
                if (result)
                {
                    var res = UOW.SaveChanges();
                    if (res > 0)
                    {
                        if (model.ProductImagePath.Count > 0)
                        {
                            var newProduct = _productRepository.Query("select * from Products where Name = {0}", model.Product.Name).FirstOrDefault();
                            var currentProductImages = _productImageRepository.Query("select * from ProductImages where ProductID = {0}", model.Product.ID).ToList();
                            _productImageRepository.Delete(currentProductImages);
                            
                            var images = new List<ProductImage>();
                            foreach (var item in model.ProductImagePath)
                            {
                                var image = new ProductImage()
                                {
                                    Caption = "new image",
                                    Name = item,
                                    ImagePath = item,
                                    FileSize = 255,
                                    CreatedDate = DateTime.Now,
                                    ModifiedDate = DateTime.Now,
                                    ModifiedBy = "toi",
                                    ProductID = newProduct.ID
                                };
                                images.Add(image);
                              
                            }
                            _productImageRepository.Add(images);
                            var res2 = UOW.SaveChanges();
                            if(res2 > 0)
                            {
                                
                                return Ok(Get(model.Product.ID));
                            }
                            
                        }
                        return Ok(Get(model.Product.ID));


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

        [Route("/admin/product/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var productImages = _productImageRepository.Query("select * from ProductImages where ProductID = {0}", id).ToList();
            _productImageRepository.Delete(productImages);
            await _productRepository.Delete(id);
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