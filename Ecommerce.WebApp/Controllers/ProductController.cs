using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Application.Services;
using Ecommerce.Application.Services.Paging;
using Ecommerce.Core.Models;
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
using SmartBreadcrumbs.Attributes;

namespace Ecommerce.WebApp.Controllers
{
    [Authorize(Roles = "member")]
    //[Area("admin")]
    //[Route("product/[action]")]

    public class ProductController : Controller
    {
        IUnitOfWork _UOW;
        IRepository<Product> _ProductRepository;
        IRepository<ProductCategory> _ProductCategoryRepository;
        IRepository<ProductImage> _ProductImageRepository;
        IRepository<Province> _provinceRepository;
        IRepository<District> _districtRepository;
        IRepository<State> _StateRepository;
        IHostingEnvironment _env;
        private UserManager<AppUser> _usermanager { get; }
        public ProductController(IUnitOfWork uow, UserManager<AppUser> usermanager, IHostingEnvironment env)
        {
            _UOW = uow;
            _ProductRepository = _UOW.GetRepository<Product>();
            _ProductCategoryRepository = _UOW.GetRepository<ProductCategory>();
            _ProductImageRepository = _UOW.GetRepository<ProductImage>();
            _provinceRepository = uow.GetRepository<Province>();
            _districtRepository = uow.GetRepository<District>();
            _StateRepository = _UOW.GetRepository<State>();
            _usermanager = usermanager;
            _env = env;
        }
        [Route("user/products")]
        [Breadcrumb("ProductList")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("user/product/list/{index}/{size}/{from}")]
        [Breadcrumb("PorductList")]
        public IPaginate<ProductListModel.ListItem> Get(int index, int size, int from)
        {
            var currentUserID = _usermanager.GetUserId(User);
            Guid id = new Guid(currentUserID);
            var productList = ProductListModel.Get(_ProductRepository, _ProductCategoryRepository, _ProductImageRepository, id);
            var page = IPaginateExtension.ToPaginate<ProductListModel.ListItem>(productList.Products.ToList(), index, size, from);
            return page;
        }
        [Route("user/product/alllist")]
        public ProductListModel Get()
        {
            var currentUserID = _usermanager.GetUserId(User);
            Guid id = new Guid(currentUserID);
            var productList = ProductListModel.Get(_ProductRepository, _ProductCategoryRepository, _ProductImageRepository,id);

            return productList;
        }
        [Route("/user/product/add")]
        [Breadcrumb("add")]
        public ProductEditModel Add()
        {
            var root = _env.WebRootPath;
            var currentUserID = _usermanager.GetUserId(User);
            string path = root + "\\uploads\\" + currentUserID;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return ProductEditModel.Create(_ProductRepository, _ProductCategoryRepository, _ProductImageRepository, _provinceRepository, _districtRepository, _StateRepository);
        }
        [HttpGet]
        [Breadcrumb("Create")]
        public IActionResult Create()
        {
            
            ViewBag.productID = 0;
            return View();

        }

        [Route("/user/product/{id?}")]
        [Breadcrumb("edit")]
        public IActionResult Edit(int id)
        {
            ViewBag.productID = id;
            return View();
        }
        [Route("/user/product/edit/{id}")]
        public ProductEditModel Get(int id)
        {
            return ProductEditModel.GetById(_ProductRepository, _ProductCategoryRepository, _ProductImageRepository, _provinceRepository, _districtRepository, _StateRepository, id);
        }
        [HttpPost]
        [Breadcrumb("save")]
        [Route("/user/product/save")]

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
                var result = model.Save(_ProductRepository, _ProductCategoryRepository, _ProductImageRepository,id);
                if (result)
                {
                    var res = _UOW.SaveChanges();
                    if (res > 0)
                    {
                        if (model.ProductImagePath.Count > 0)
                        {
                            var newProduct = _ProductRepository.Query("select * from Products where Name = {0}", model.Product.Name).FirstOrDefault();
                            var currentProductImages = _ProductImageRepository.Query("select * from ProductImages where ProductID = {0}", model.Product.ID).ToList();
                            _ProductImageRepository.Delete(currentProductImages);

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
                            _ProductImageRepository.Add(images);
                            var res2 = _UOW.SaveChanges();
                            if (res2 > 0)
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

        [Route("/user/product/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var productImages = _ProductImageRepository.Query("select * from ProductImages where ProductID = {0}", id).ToList();
            _ProductImageRepository.Delete(productImages);
            await _ProductRepository.Delete(id);
            var res = _UOW.SaveChanges();
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

        private StatusMessage GetSuccessMessage(string message)
        {
            return GetMessage(message, StatusMessage.Success);
        }
        private StatusMessage GetErrorMessage(string message)
        {
            return GetMessage(message, StatusMessage.Error);
        }
        private StatusMessage GetMessage(string message, string type)
        {
            var Status = new StatusMessage
            {
                Type = type,
                Body = message
            };
            return Status;
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