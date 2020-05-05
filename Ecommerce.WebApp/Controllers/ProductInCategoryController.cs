using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Application.Services;
using Ecommerce.Data.Entities;
using Ecommerce.WebApp.Extension;
using Ecommerce.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.WebApp.Controllers
{
    public class ProductInCategoryController : Controller
    {
        IUnitOfWork _UOW;
        IRepository<Product> _ProductRepository;
        IRepository<ProductCategory> _ProductCategoryRepository;
        IRepository<ProductImage> _ProductImageRepository;
        IRepository<Province> _provinceRepository;
        IRepository<District> _districtRepository;
        IRepository<State> _StateRepository;
        public ProductInCategoryController(IUnitOfWork uow)
        {
            _UOW = uow;
            _ProductRepository = _UOW.GetRepository<Product>();
            _ProductCategoryRepository = _UOW.GetRepository<ProductCategory>();
            _ProductImageRepository = _UOW.GetRepository<ProductImage>();
            _provinceRepository = uow.GetRepository<Province>();
            _districtRepository = uow.GetRepository<District>();
            _StateRepository = _UOW.GetRepository<State>();
        }
        [Route("productincategory/{cateid}")]
        public IActionResult Index(int cateid)
        {
            ViewBag.cateid = cateid;
            return View();
        }
        
        [Route("getbycategoryid/{categoryid}")]
        public ProductInCategoryModel GetByID(int categoryid)
        {
            
            var productincategory = ProductInCategoryModel.GetByCategoryID(_UOW,_ProductRepository, _ProductCategoryRepository, _ProductImageRepository, _provinceRepository, _districtRepository, _StateRepository, categoryid);
            return productincategory;
        }
        [Route("filtercondition/{condition}")]
        public ProductInCategoryModel FilterCondition(int condition)
        {

            var productincategory = ProductInCategoryModel.GetByCategoryID(_UOW, _ProductRepository, _ProductCategoryRepository, _ProductImageRepository, _provinceRepository, _districtRepository, _StateRepository, 0);
            return productincategory;
        }
    }
}