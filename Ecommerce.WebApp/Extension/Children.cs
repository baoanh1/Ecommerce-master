using Ecommerce.Application.Services;
using Ecommerce.Data.Entities;
using Ecommerce.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.WebApp.Extension
{
    public class Children
    {
        IRepository<ProductCategory> _productCategoryRepository;
        private IUnitOfWork _UOW;
        public Children(IUnitOfWork uow)
        {
            _UOW = uow;
            _productCategoryRepository = uow.GetRepository<ProductCategory>();
        }
        public IList<Item> GetLayers(int parentId=0)
        {
            var cate = _productCategoryRepository.GetByID(parentId);
            var Categories = _productCategoryRepository.GetAll()
                   .OrderBy(u => u.ID)
                   .Select(u => new Item
                   {
                       ID = u.ID,
                       Name = u.Name,
                       ParentID = u.ParentID,
                       ParentName = cate == null||cate.ParentID==0 ? "Root" : _productCategoryRepository.GetByID(cate.ParentID).Name
                   }).ToList();
            return GetChildren(Categories, parentId);
        }

        private IList<Item> GetChildren(IList<Item> source, int parentId)
        {
            var children = source.Where(x => x.ParentID == parentId).ToList();
            children.ForEach(x => x.ChildLayers = GetChildren(source, x.ID));
            return children;
        }
        public class Item
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Image { get; set; }
            public string MetaTitle { get; set; }

            public int ParentID { get; set; }
            public string ParentName { get; set; }

            public int? DisplayOrder { get; set; }


            public string SeoTitle { get; set; }

            public DateTime CreatedDate { get; set; }

            public DateTime ModifiedDate { get; set; }


            public string ModifiedBy { get; set; }

            public string MetaKeywords { get; set; }

            public string MetaDescriptions { get; set; }

            public Status Status { get; set; }

            public bool? ShowOnHome { get; set; }

            public IList<Item> ChildLayers { get; set; }
        }
    }
}
