using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Application.Services;
using Ecommerce.Data.Entities;
using Ecommerce.WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.WebApp.Controllers
{
    public class ProfileController : Controller
    {
        IUnitOfWork _UOW;
        IRepository<Product> _ProductRepository;
        IRepository<ProductCategory> _ProductCategoryRepository;
        IRepository<ProductImage> _ProductImageRepository;
        IRepository<Province> _provinceRepository;
        IRepository<District> _districtRepository;
        IRepository<State> _StateRepository;
        IRepository<ChatRoom> _ChatRoomRepository;
        private UserManager<AppUser> _usermanager { get; }
        public ProfileController(IUnitOfWork uow, UserManager<AppUser> usermanager)
        {
            _UOW = uow;
            _ProductRepository = _UOW.GetRepository<Product>();
            _ProductCategoryRepository = _UOW.GetRepository<ProductCategory>();
            _ProductImageRepository = _UOW.GetRepository<ProductImage>();
            _provinceRepository = uow.GetRepository<Province>();
            _districtRepository = uow.GetRepository<District>();
            _StateRepository = _UOW.GetRepository<State>();
            _ChatRoomRepository = _UOW.GetRepository<ChatRoom>(); ;
            _usermanager = usermanager;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Route("getprofile")]

        public ProfileModel GetProfile()
        {
            var currentUserID = _usermanager.GetUserId(User);
            Guid id = new Guid(currentUserID);
            var buyproductids = _ChatRoomRepository.GetAll().Where(x => x.SenderID == id).Select(i=>i.ProductID).ToList();
            var profile = ProfileModel.Get(_ProductRepository, _ProductCategoryRepository, _ProductImageRepository, _provinceRepository, _districtRepository, _StateRepository, _usermanager, id, buyproductids);
            return profile;
        }
    }
}