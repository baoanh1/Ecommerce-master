using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ecommerce.Data.Migrations
{
    public partial class reset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRoleClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    Name = table.Column<string>(nullable: false),
                    NormalizedName = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUserLogins",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: true),
                    ProviderKey = table.Column<string>(nullable: true),
                    ProviderDisplayName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserLogins", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "AppUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserRoles", x => new { x.UserId, x.RoleId });
                });

            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    UserName = table.Column<string>(nullable: false),
                    NormalizedUserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    NormalizedEmail = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserTokens", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "ChatRooms",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserIDs = table.Column<string>(nullable: true),
                    SenderID = table.Column<Guid>(nullable: false),
                    ReceiverID = table.Column<Guid>(nullable: false),
                    ProductID = table.Column<int>(nullable: false),
                    SenderStatus = table.Column<int>(nullable: false),
                    ReceiverStatus = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatRooms", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Districts",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProvinceID = table.Column<int>(nullable: false),
                    Code = table.Column<int>(maxLength: 200, nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ChatRoomID = table.Column<int>(nullable: false),
                    SenderID = table.Column<int>(nullable: false),
                    Mes = table.Column<string>(nullable: true),
                    Status = table.Column<int>(maxLength: 4, nullable: false, defaultValue: 1)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategorys",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false),
                    Image = table.Column<string>(nullable: true),
                    MetaTitle = table.Column<string>(nullable: true),
                    ParentID = table.Column<int>(nullable: false, defaultValue: 0)
                        .Annotation("Sqlite:Autoincrement", true),
                    DisplayOrder = table.Column<int>(nullable: true),
                    SeoTitle = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    MetaKeywords = table.Column<string>(nullable: true),
                    MetaDescriptions = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    ShowOnHome = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategorys", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false),
                    ImagePath = table.Column<string>(maxLength: 250, nullable: true),
                    ProductID = table.Column<int>(nullable: false, defaultValue: 0)
                        .Annotation("Sqlite:Autoincrement", true),
                    DisplayOrder = table.Column<int>(nullable: false),
                    Caption = table.Column<string>(maxLength: 250, nullable: true),
                    IsDefault = table.Column<bool>(nullable: false, defaultValue: true),
                    FileSize = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserID = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    MetaTitle = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ProvinceID = table.Column<int>(nullable: false),
                    DistrictID = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false, defaultValue: 0m),
                    PromotionPrice = table.Column<decimal>(nullable: false),
                    Quantity = table.Column<int>(nullable: false, defaultValue: 0)
                        .Annotation("Sqlite:Autoincrement", true),
                    categoryID = table.Column<int>(nullable: false, defaultValue: 0)
                        .Annotation("Sqlite:Autoincrement", true),
                    Detail = table.Column<string>(nullable: true),
                    Warranty = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    MetaKeywords = table.Column<string>(nullable: true),
                    MetaDescriptions = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    StateID = table.Column<int>(nullable: false),
                    TopHot = table.Column<bool>(nullable: false),
                    ViewCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Provinces",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<int>(maxLength: 200, nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provinces", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Receivers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MessageID = table.Column<int>(nullable: false),
                    ReceiverID = table.Column<int>(nullable: false),
                    ReadAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receivers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "AppRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[] { 2, "MemberClaim", "Menber role claim", new Guid("c04a0585-befc-4aaa-9aee-4cf74d3cc08d") });

            migrationBuilder.InsertData(
                table: "AppRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[] { 1, "AdminClaim", "Admin role claim", new Guid("221a6427-71e6-4efc-9c48-73bb6609cd85") });

            migrationBuilder.InsertData(
                table: "AppRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("221a6427-71e6-4efc-9c48-73bb6609cd85"), "332637b4-7921-4c77-8f0f-7070e36347b6", "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AppRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("c04a0585-befc-4aaa-9aee-4cf74d3cc08d"), "584935ed-d8ec-4427-940e-fdcf8500e994", "member", "MEMBER" });

            migrationBuilder.InsertData(
                table: "AppUserClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "UserId" },
                values: new object[] { 1, "AdminClaim", "Admin role claim", new Guid("00000000-0000-0000-0000-000000000000") });

            migrationBuilder.InsertData(
                table: "AppUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { new Guid("64d8abab-2f04-4856-a9a5-d17ef35628b1"), new Guid("221a6427-71e6-4efc-9c48-73bb6609cd85") });

            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("64d8abab-2f04-4856-a9a5-d17ef35628b1"), 0, "14c6e567-ae7a-49a0-90a2-4b716ac13d2c", "phungnhatphu4@gmail.com", true, null, null, false, null, "PHUNGNHATPHU4@GMAIL>COM", "ADMIN", "AQAAAAEAACcQAAAAEEpzM8IQVlq1vq3S7GGzTgEv2VN37FAqvf3gBrISe/PNgjLi31pE0pRJK/ttsMoILw==", null, false, "a799990b-25f0-4359-9087-b73c5184073a", false, "admin" });

            migrationBuilder.InsertData(
                table: "Districts",
                columns: new[] { "ID", "Code", "Name", "ProvinceID" },
                values: new object[] { 4, 0, "Quang Xuong", 4 });

            migrationBuilder.InsertData(
                table: "Districts",
                columns: new[] { "ID", "Code", "Name", "ProvinceID" },
                values: new object[] { 5, 0, "Quan 1", 3 });

            migrationBuilder.InsertData(
                table: "Districts",
                columns: new[] { "ID", "Code", "Name", "ProvinceID" },
                values: new object[] { 6, 0, "Quan 2", 3 });

            migrationBuilder.InsertData(
                table: "Districts",
                columns: new[] { "ID", "Code", "Name", "ProvinceID" },
                values: new object[] { 7, 0, "Uong Bi", 2 });

            migrationBuilder.InsertData(
                table: "Districts",
                columns: new[] { "ID", "Code", "Name", "ProvinceID" },
                values: new object[] { 8, 0, "Ha Long", 2 });

            migrationBuilder.InsertData(
                table: "Districts",
                columns: new[] { "ID", "Code", "Name", "ProvinceID" },
                values: new object[] { 1, 0, "Cau Giay", 1 });

            migrationBuilder.InsertData(
                table: "Districts",
                columns: new[] { "ID", "Code", "Name", "ProvinceID" },
                values: new object[] { 3, 0, "Thieu Hoa", 4 });

            migrationBuilder.InsertData(
                table: "Districts",
                columns: new[] { "ID", "Code", "Name", "ProvinceID" },
                values: new object[] { 2, 0, "BA Dinh", 1 });

            migrationBuilder.InsertData(
                table: "ProductCategorys",
                columns: new[] { "ID", "CreatedDate", "DisplayOrder", "Image", "MetaDescriptions", "MetaKeywords", "MetaTitle", "ModifiedBy", "ModifiedDate", "Name", "SeoTitle", "ShowOnHome", "Status" },
                values: new object[] { 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "May giat", null, null, 0 });

            migrationBuilder.InsertData(
                table: "ProductCategorys",
                columns: new[] { "ID", "CreatedDate", "DisplayOrder", "Image", "MetaDescriptions", "MetaKeywords", "MetaTitle", "ModifiedBy", "ModifiedDate", "Name", "ParentID", "SeoTitle", "ShowOnHome", "Status" },
                values: new object[] { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "tivi", 1, null, null, 0 });

            migrationBuilder.InsertData(
                table: "ProductCategorys",
                columns: new[] { "ID", "CreatedDate", "DisplayOrder", "Image", "MetaDescriptions", "MetaKeywords", "MetaTitle", "ModifiedBy", "ModifiedDate", "Name", "ParentID", "SeoTitle", "ShowOnHome", "Status" },
                values: new object[] { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tu lanh", 2, null, null, 0 });

            migrationBuilder.InsertData(
                table: "ProductCategorys",
                columns: new[] { "ID", "CreatedDate", "DisplayOrder", "Image", "MetaDescriptions", "MetaKeywords", "MetaTitle", "ModifiedBy", "ModifiedDate", "Name", "ParentID", "SeoTitle", "ShowOnHome", "Status" },
                values: new object[] { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Labtop", 1, null, null, 0 });

            migrationBuilder.InsertData(
                table: "ProductCategorys",
                columns: new[] { "ID", "CreatedDate", "DisplayOrder", "Image", "MetaDescriptions", "MetaKeywords", "MetaTitle", "ModifiedBy", "ModifiedDate", "Name", "SeoTitle", "ShowOnHome", "Status" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dien thoai", null, null, 0 });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ID", "Code", "CreatedDate", "Description", "Detail", "DistrictID", "MetaDescriptions", "MetaKeywords", "MetaTitle", "ModifiedBy", "ModifiedDate", "Name", "PromotionPrice", "ProvinceID", "StateID", "Status", "TopHot", "UserID", "ViewCount", "Warranty", "categoryID" },
                values: new object[] { 4, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 0, null, null, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Iphone 9", 0m, 0, 3, 0, false, new Guid("64d8abab-2f04-4856-a9a5-d17ef35628b1"), 0, 0, 4 });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ID", "Code", "CreatedDate", "Description", "Detail", "DistrictID", "MetaDescriptions", "MetaKeywords", "MetaTitle", "ModifiedBy", "ModifiedDate", "Name", "PromotionPrice", "ProvinceID", "StateID", "Status", "TopHot", "UserID", "ViewCount", "Warranty", "categoryID" },
                values: new object[] { 3, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 0, null, null, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Iphone 8", 0m, 0, 3, 0, false, new Guid("64d8abab-2f04-4856-a9a5-d17ef35628b1"), 0, 0, 3 });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ID", "Code", "CreatedDate", "Description", "Detail", "DistrictID", "MetaDescriptions", "MetaKeywords", "MetaTitle", "ModifiedBy", "ModifiedDate", "Name", "PromotionPrice", "ProvinceID", "StateID", "Status", "TopHot", "UserID", "ViewCount", "Warranty", "categoryID" },
                values: new object[] { 2, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 0, null, null, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Iphone 7", 0m, 0, 2, 0, false, new Guid("64d8abab-2f04-4856-a9a5-d17ef35628b1"), 0, 0, 2 });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ID", "Code", "CreatedDate", "Description", "Detail", "DistrictID", "MetaDescriptions", "MetaKeywords", "MetaTitle", "ModifiedBy", "ModifiedDate", "Name", "PromotionPrice", "ProvinceID", "StateID", "Status", "TopHot", "UserID", "ViewCount", "Warranty", "categoryID" },
                values: new object[] { 1, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 0, null, null, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Iphone 6", 0m, 0, 1, 0, false, new Guid("64d8abab-2f04-4856-a9a5-d17ef35628b1"), 0, 0, 1 });

            migrationBuilder.InsertData(
                table: "Provinces",
                columns: new[] { "ID", "Code", "Name" },
                values: new object[] { 1, 100000, "Ha Noi" });

            migrationBuilder.InsertData(
                table: "Provinces",
                columns: new[] { "ID", "Code", "Name" },
                values: new object[] { 2, 100000, "Quang Ninh" });

            migrationBuilder.InsertData(
                table: "Provinces",
                columns: new[] { "ID", "Code", "Name" },
                values: new object[] { 3, 200000, "HCM" });

            migrationBuilder.InsertData(
                table: "Provinces",
                columns: new[] { "ID", "Code", "Name" },
                values: new object[] { 4, 200000, "Thanh Hoa" });

            migrationBuilder.InsertData(
                table: "States",
                columns: new[] { "ID", "Name" },
                values: new object[] { 1, "Moi 100%" });

            migrationBuilder.InsertData(
                table: "States",
                columns: new[] { "ID", "Name" },
                values: new object[] { 2, "Nhu moi" });

            migrationBuilder.InsertData(
                table: "States",
                columns: new[] { "ID", "Name" },
                values: new object[] { 3, "Cũ" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppRoleClaims");

            migrationBuilder.DropTable(
                name: "AppRoles");

            migrationBuilder.DropTable(
                name: "AppUserClaims");

            migrationBuilder.DropTable(
                name: "AppUserLogins");

            migrationBuilder.DropTable(
                name: "AppUserRoles");

            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropTable(
                name: "AppUserTokens");

            migrationBuilder.DropTable(
                name: "ChatRooms");

            migrationBuilder.DropTable(
                name: "Districts");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "ProductCategorys");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Provinces");

            migrationBuilder.DropTable(
                name: "Receivers");

            migrationBuilder.DropTable(
                name: "States");
        }
    }
}
