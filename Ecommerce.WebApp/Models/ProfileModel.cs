using Ecommerce.Application.Services;
using Ecommerce.Data.Entities;
using Ecommerce.Data.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.WebApp.Models
{
    public class ProfileModel
    {
        public ProfileUser Profile { get; set; }
        public IList<ListProductView> Products { get; set; } = new List<ListProductView>();
        public IList<ListProductView> BuyProducts { get; set; } = new List<ListProductView>();
        public IList<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
        public static ProfileModel Get(IRepository<Product> ProductRepository,
                                           IRepository<ProductCategory> ProductCategoryRepository,
                                           IRepository<ProductImage> productImageRepository,
                                           IRepository<Province> ProvinceRepository,
                                           IRepository<District> DistrictRepository,
                                           IRepository<State> StateRepository,
                                           UserManager<AppUser> usermanager,Guid userid, List<int> buyproductids)
        {
            var info = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");
            var user = usermanager.FindByIdAsync(userid.ToString()).Result;
      
            var model = new ProfileModel
            {
                Products = ProductRepository.GetAll()
                   .OrderBy(u => u.ID).Where(x => x.UserID == userid)
                   .Select(u => new ListProductView
                   {
                       ID = u.ID,
                       Name = u.Name,
                       UserID = u.UserID.ToString(),
                       Price = String.Format(info, "{0:c}", u.Price),
                       Images = productImageRepository.GetAll().Where(x => x.ProductID == u.ID).ToList(),
                       categoryName = ProductCategoryRepository.GetByID(u.categoryID).Name,
                       ProvinceName = ProvinceRepository.GetByID(u.ProvinceID).Name,
                       DistrictName = DistrictRepository.GetByID(u.DistrictID).Name,
                       StateName = StateRepository.GetByID(u.StateID).Name
                   }).ToList(),
                BuyProducts = ProductRepository.GetAll()
                   .OrderBy(u => u.ID).Where(x => buyproductids.Contains(x.ID))
                   .Select(u => new ListProductView
                   {
                       ID = u.ID,
                       Name = u.Name,
                       UserID = u.UserID.ToString(),
                       Price = String.Format(info, "{0:c}", u.Price),
                       Images = productImageRepository.GetAll().Where(x => x.ProductID == u.ID).ToList(),
                       categoryName = ProductCategoryRepository.GetByID(u.categoryID).Name,
                       ProvinceName = ProvinceRepository.GetByID(u.ProvinceID).Name,
                       DistrictName = DistrictRepository.GetByID(u.DistrictID).Name,
                       StateName = StateRepository.GetByID(u.StateID).Name
                   }).ToList(),
                ProductCategories = ProductCategoryRepository.GetAll().ToList(),
                Profile = new ProfileUser { 
                    ID = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                   
                }

            };
            return model;
        }
       
        public class ProfileUser
        {
            public Guid ID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            [Display(Name = "User Name")]
            [RegularExpression(@"[^\s]+")]
            public string UserName { get; set; }
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }
        public class ListProductView
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
