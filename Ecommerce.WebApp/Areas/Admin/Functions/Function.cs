using Ecommerce.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.WebApp.Areas.Admin.Functions
{
    public static class Function
    {
        public static void cate_parent(List<ProductCategory> data, ref string htmlstring, int parent = 0, string str = "--", int select = 0)
        {
            foreach (var item in data)
            {
                var id = item.ID;
                var name = item.Name;

                if (item.ParentID == parent)
                {
                    if (select != 0 && id == select)
                    {
                        htmlstring += "<option value=" + id + " selected='selected'>" + str + name + "</option>";

                    }
                    else
                    {
                        htmlstring += "<option value=" + id + ">" + str + name + "</option>";
                    }

                    cate_parent(data, ref htmlstring, id, str + "--", select);



                }

            }
        }
    }
}
