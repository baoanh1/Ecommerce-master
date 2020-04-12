using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.ViewModels.Catalog.Product
{
    public class PageViewModel<T>
    {
        List<T> Items { get; set; }
        public int ToTalRecord { get; set; }
    }
}
