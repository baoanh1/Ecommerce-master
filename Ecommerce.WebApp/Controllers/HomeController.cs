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
        public HomeController(ILogger<HomeController> logger, IUnitOfWork uow)
        {
            _logger = logger;
            _UOW = uow;
            _ProductRepository = _UOW.GetRepository<Product>();
            _productCategoryRepository = uow.GetRepository<ProductCategory>();
            _ProductImageRepository = _UOW.GetRepository<ProductImage>();
            _provinceRepository = _UOW.GetRepository<Province>();
            _districtRepository = _UOW.GetRepository<District>();
            _StateRepository = _UOW.GetRepository<State>();
        }
        
        public IActionResult Index()
        {
            var products = ProductViewModel.Get(_ProductRepository,
                                                _productCategoryRepository,
                                                _ProductImageRepository,
                                                _provinceRepository,
                                                _districtRepository,
                                                _StateRepository);
            return View(products);
        }

        public IActionResult ProductDetail(int id)
        {
            var model = ProductDetailModel.GetByID(_ProductRepository,
                                                _productCategoryRepository,
                                                _ProductImageRepository,
                                                _provinceRepository,
                                                _districtRepository,
                                                _StateRepository, id);
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
