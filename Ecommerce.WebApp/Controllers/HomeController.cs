using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ecommerce.WebApp.Models;
using Ecommerce.Application.Services;
using Ecommerce.Data.Entities;
using SmartBreadcrumbs.Attributes;
using SmartBreadcrumbs.Nodes;
using Ecommerce.WebApp.Areas.Admin.Functions;
using Ecommerce.WebApp.Extension;
using static Ecommerce.WebApp.Models.FilterChildren;
using Ecommerce.Application.Services.Paging;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        IRepository<Product> _ProductRepository;
        IRepository<ProductCategory> _productCategoryRepository;
        IRepository<ProductImage> _ProductImageRepository;
        IRepository<Province> _provinceRepository;
        IRepository<District> _districtRepository;
        IRepository<State> _StateRepository;
        private IUnitOfWork _UOW;
        private UserManager<AppUser> _usermanager { get; }
        public HomeController(ILogger<HomeController> logger, IUnitOfWork uow, UserManager<AppUser> usermanager)
        {
            _logger = logger;
            _UOW = uow;
            _ProductRepository = _UOW.GetRepository<Product>();
            _productCategoryRepository = uow.GetRepository<ProductCategory>();
            _ProductImageRepository = _UOW.GetRepository<ProductImage>();
            _provinceRepository = _UOW.GetRepository<Province>();
            _districtRepository = _UOW.GetRepository<District>();
            _StateRepository = _UOW.GetRepository<State>();
            _usermanager = usermanager;
        }
        public ActionResult SearchForm()
        {
            return View();
        }
        [Route("searchform")]
        public ProductViewModel Search()
        {
            var products = ProductViewModel.Get(_ProductRepository,
                                                _productCategoryRepository,
                                                _ProductImageRepository,
                                                _provinceRepository,
                                                _districtRepository,
                                                _StateRepository,
                                                _usermanager);
            return products;
        }
        [Route("Home/{index}/{size}")]
        public IPaginate<ProductViewModel.ListItem> GetPaging(int index, int size, int from=0)
        {
            var products = ProductViewModel.Get(_ProductRepository,
                                                _productCategoryRepository,
                                                _ProductImageRepository,
                                                _provinceRepository,
                                                _districtRepository,
                                                _StateRepository, _usermanager);
            var page = IPaginateExtension.ToPaginate<ProductViewModel.ListItem>(products.Products, index, size, from);
            return page;
        }
        [Breadcrumb("Home")]
        public IActionResult Index()
        {
            
            //var products = ProductViewModel.Get(_ProductRepository,
            //                                    _productCategoryRepository,
            //                                    _ProductImageRepository,
            //                                    _provinceRepository,
            //                                    _districtRepository,
            //                                    _StateRepository);
            return View();
        }
        [Route("category/{categoryid}")]
        [Breadcrumb("ViewData.categoryid")]
        public IActionResult ProductCategoryDetail(int categoryid)
        {
            var products = ProductViewModel.GetByCategoryID(_ProductRepository,
                                                _productCategoryRepository,
                                                _ProductImageRepository,
                                                _provinceRepository,
                                                _districtRepository,
                                                _StateRepository, categoryid);

            var categoryname = _productCategoryRepository.GetByID(categoryid).Name;
            ViewData["categoryid"] = "category-"+ categoryname;
           
            return View(products);
        }
        [Breadcrumb("ViewData.productid")]
        [Route("productdetail/{id}")]
        public IActionResult ProductDetail(int id)
        {
            var model = ProductDetailModel.GetByID(_ProductRepository,
                                                _productCategoryRepository,
                                                _ProductImageRepository,
                                                _provinceRepository,
                                                _districtRepository,
                                                _StateRepository, id);

            var productname = _ProductRepository.GetByID(id).Name;
            ViewData["productid"] = "product-" + productname;
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public static IList<Item> GetParentChildrenCategory(IRepository<ProductCategory> productCategoryRepository)
        {
            var Categories = productCategoryRepository.GetAll()
                   .OrderBy(u => u.ID)
                   .Select(u => new Item
                   {
                       ID = u.ID,
                       Name = u.Name,
                       ParentID = u.ParentID
                   }).ToList();

            IList<Item> getChildren = GetChildren(Categories, 0);
            return getChildren;
        }
        private static IList<Item> GetChildren(IList<Item> source, int parentId)
        {
            var children = source.OrderBy(u => u.ID).Where(x => x.ParentID == parentId).ToList();
            //GetChildren is called recursively again for every child found
            //and this process repeats until no childs are found for given node, 
            //in which case an empty list is returned
            children.ForEach(x => x.ChildLayers = GetChildren(source, x.ID));
            return children;
        }
    }
}
