using Ecommerce.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Data.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductCategory>().HasData
            (
                new ProductCategory { ID = 1, ParentID= 0, Name = "Dien thoai" },
                new ProductCategory { ID = 2, ParentID = 1, Name = "Labtop" },
                new ProductCategory { ID = 3, ParentID = 1, Name = "tivi" },
                new ProductCategory { ID = 4, ParentID = 2, Name = "Tu lanh" },
                new ProductCategory { ID = 5, ParentID = 0, Name = "May giat" }
            );

           
          



            var ADMIN_ID = Guid.NewGuid();
            // any guid, but nothing is against to use the same one
            var ROLE_ID = Guid.NewGuid();
            var ROLE_ID1 = Guid.NewGuid();
            modelBuilder.Entity<AppRole>().HasData(
            new AppRole
            {
                Id = ROLE_ID,
                Name = "admin",
                NormalizedName = "ADMIN"
            },
            new AppRole
            {
                Id = ROLE_ID1,
                Name = "member",
                NormalizedName = "MEMBER"
            }
            );
            
            var hasher = new PasswordHasher<AppUser>();
            modelBuilder.Entity<AppUser>().HasData(new AppUser
            {
                Id = ADMIN_ID,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "phungnhatphu4@gmail.com",
                NormalizedEmail = "PHUNGNHATPHU4@GMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "NxP@2020"),
                SecurityStamp = Guid.NewGuid().ToString("D")
            });

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = ROLE_ID,
                UserId = ADMIN_ID
            });
            modelBuilder.Entity <IdentityRoleClaim<Guid>>().HasData(new IdentityRoleClaim<Guid>
            {
                Id = 1,
                RoleId = ROLE_ID,
                ClaimType = "AdminClaim",
                ClaimValue = "Admin role claim"
            },
            new IdentityRoleClaim<Guid>
            {
                Id = 2,
                RoleId = ROLE_ID1,
                ClaimType = "MemberClaim",
                ClaimValue = "Menber role claim"
            });
            modelBuilder.Entity<IdentityUserClaim<Guid>>().HasData(
            new IdentityUserClaim<Guid>
            {
                Id = 1,
                ClaimType = "AdminClaim",
                ClaimValue = "Admin role claim"
            });
            modelBuilder.Entity<Product>().HasData
          (
              new Product { ID = 1, UserID = ADMIN_ID, Name = "Iphone 6", categoryID = 1, StateID = 1},
              new Product { ID = 2, UserID = ADMIN_ID, Name = "Iphone 7", categoryID = 2, StateID = 2 },
              new Product { ID = 3, UserID = ADMIN_ID, Name = "Iphone 8", categoryID = 3, StateID = 3 },
              new Product { ID = 4, UserID = ADMIN_ID, Name = "Iphone 9", categoryID = 4, StateID = 3 }
          );
            modelBuilder.Entity<ProductImage>().HasData
         (
             new ProductImage { ID = 1, Name = "image 1", ImagePath= "/CMS/Content/new/14818053_IMG_20200403_143941.jpg" },
             new ProductImage { ID = 2, Name = "image 2", ImagePath = "/CMS/Content/new/14818053_IMG_20200403_143941.jpg" },
             new ProductImage { ID = 3, Name = "image 3", ImagePath = "/CMS/Content/new/14818053_IMG_20200403_143941.jpg" },
             new ProductImage { ID = 4, Name = "image 4", ImagePath = "/CMS/Content/new/14818053_IMG_20200403_143941.jpg" }
         );
            
            modelBuilder.Entity<Province>().HasData
         (
             new Province { ID = 1, Name = "Ha Noi", Code=100000},
             new Province { ID = 2, Name = "Quang Ninh", Code = 100000 },
             new Province { ID = 3, Name = "HCM", Code = 200000 },
             new Province { ID = 4, Name = "Thanh Hoa", Code = 200000 }
         );
            modelBuilder.Entity<District>().HasData
        (
            new District { ID = 1, Name = "Cau Giay", ProvinceID = 1 },
            new District { ID = 2, Name = "BA Dinh", ProvinceID = 1 },
            new District { ID = 3, Name = "Thieu Hoa", ProvinceID = 4 },
            new District { ID = 4, Name = "Quang Xuong", ProvinceID = 4 },
            new District { ID = 5, Name = "Quan 1", ProvinceID = 3 },
            new District { ID = 6, Name = "Quan 2", ProvinceID = 3 },
            new District { ID = 7, Name = "Uong Bi", ProvinceID = 2 },
            new District { ID = 8, Name = "Ha Long", ProvinceID = 2 }
        );
            modelBuilder.Entity<State>().HasData
        (
            new State { ID = 1, Name = "Moi 100%" },
            new State { ID = 2, Name = "Nhu moi"},
            new State { ID = 3, Name = "Cũ"}
         );
        }
    }
}
